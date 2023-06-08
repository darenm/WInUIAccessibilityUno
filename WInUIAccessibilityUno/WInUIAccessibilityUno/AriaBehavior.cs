using Microsoft.UI.Xaml.Automation;
using Microsoft.Xaml.Interactivity;

namespace WInUIAccessibilityUno
{
    public class AriaBehavior : SafeBehaviorBase<FrameworkElement>
    {
        private static class ElementTypes
        {
            internal const string Input = "INPUT";
            internal const string Button = "BUTTON";
            internal const string TextArea = "TEXTAREA";
            internal const string Image = "IMG";
        }
        public string LabelOverride
        {
            get { return (string)GetValue(LabelOverrideProperty); }
            set { SetValue(LabelOverrideProperty, value); }
        }

        public static readonly DependencyProperty LabelOverrideProperty =
            DependencyProperty.Register("LabelOverride", typeof(string), typeof(AriaBehavior), new PropertyMetadata(""));

        public string Alt
        {
            get { return (string)GetValue(AltProperty); }
            set { SetValue(AltProperty, value); }
        }

        public static readonly DependencyProperty AltProperty =
            DependencyProperty.Register("Alt", typeof(string), typeof(AriaBehavior), new PropertyMetadata(""));


        protected override void OnSetup()
        {
            base.OnSetup();

#if !WINDOWS

            if (AssociatedObject.IsLoaded)
            {
                updateLabel();
            }
            else
            {
                AssociatedObject.Loaded += AssociatedObject_Loaded;
            }
#endif        
        }

        protected override void OnCleanup()
        {
            base.OnCleanup();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
#if !WINDOWS
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            updateLabel();
#endif
        }
#if !WINDOWS


        private string _js(string alt) => @"function ariaFindChildType(parent, childType) {
    var children = parent.children;
    for (var i = 0; i < children.length; i++) {
        {
            var child = children[i];
            if (child.tagName === childType) { return child; }
            var result = ariaFindChildType(child, childType);
            if (result) {
                return result;
            }
        }
    }
    return null;
}
function ariaApplyLabelToChild(parent, childType, label) {
    var child = ariaFindChildType(parent, childType);
    if (child) {
        child.setAttribute(""aria-label"", label);
    }
}
function ariaIgnoreDecorativeImage(parent) {
    var child = ariaFindChildType(parent, ""IMG"");
    if (child) {
        child.setAttribute(""alt"", """ + Alt + @""");
        child.setAttribute(""role"", ""presentation"");
    }
}
";
        private void updateLabel()
        {
            string? labelText = null;
            var elementTypes = new List<string>();
            var elementId = AssociatedObject.GetHtmlId();
            // Some initial attempts to resolve a label for the element
            switch (AssociatedObject)
            {
                case PasswordBox passwordBox:
                    elementTypes.Add(ElementTypes.Input);
                    labelText = string.IsNullOrEmpty(passwordBox.PlaceholderText) ? "Password" : $"Password, {passwordBox.PlaceholderText}";
                    break;

                case TextBox textBox:
                    elementTypes.Add(ElementTypes.Input);
                    elementTypes.Add(ElementTypes.TextArea);
                    labelText = string.IsNullOrEmpty(textBox.PlaceholderText) ? "Text" : textBox.PlaceholderText;
                    break;

                case CheckBox checkBox:
                    elementTypes.Add(ElementTypes.Input);
                    if (!string.IsNullOrEmpty(checkBox.Content.ToString()))
                    {
                        if (checkBox.Content is TextBlock checkBoxTextBlock)
                        {
                            labelText = $"CheckBox, {checkBoxTextBlock.Text}";
                        }
                    }
                    break;

                case Image image:
                    elementTypes.Add(ElementTypes.Image);
                    labelText = "Image";
                    break;

                default:
                    return;
            }

            // If there is an override value, always use that
            if (!string.IsNullOrEmpty(LabelOverride))
            {
                labelText = LabelOverride;
            }

            // What other ways are there to get the textbox's label?
            if (string.IsNullOrEmpty(labelText))
            {
                var labeledBy = AutomationProperties.GetLabeledBy(AssociatedObject);
                if (labeledBy is TextBlock textBlock)
                {
                    labelText = textBlock.Text;
                }
            }

            if (string.IsNullOrEmpty(labelText))
            {
                var name = AutomationProperties.GetName(AssociatedObject);
                if (!string.IsNullOrEmpty(name))
                {
                    labelText = name;
                }
            }

            // This caters for controls such as TextBox that may be rendered either with a Input element or a TextArea element
            if (!string.IsNullOrEmpty(labelText))
            {
                foreach (var eType in elementTypes)
                {
                    string javascript;
                    switch (eType)
                    {
                        case ElementTypes.Image:
                            javascript = @$"{_js(Alt)} let elem = document.getElementById(""{elementId}""); ariaIgnoreDecorativeImage(elem); ";
                            break;

                        default:
                            javascript = @$"{_js(Alt)} let elem = document.getElementById(""{elementId}""); ariaApplyLabelToChild(elem, ""{eType}"", ""{labelText}""); ";
                            break;
                    }
                    AssociatedObject.ExecuteJavascript(javascript);
                }
            }
        }

#endif

    }

    #region Framework
    public abstract class SafeBehaviorBase<T> : Behavior<T> where T : FrameworkElement
    {
        protected bool IsSetup { get; private set; }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Unloaded += associatedObjectUnloaded;
            AssociatedObject.Loaded += associatedObjectLoaded;

            if (AssociatedObject.IsLoaded)
            {
                setup();
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Unloaded -= associatedObjectUnloaded;
            AssociatedObject.Loaded -= associatedObjectLoaded;

            cleanup();

            base.OnDetaching();
        }

        void associatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            setup();
        }

        void associatedObjectUnloaded(object sender, RoutedEventArgs e)
        {
            cleanup();
        }

        void cleanup()
        {
            if (IsSetup)
            {
                OnCleanup();
                IsSetup = false;
            }
        }

        void setup()
        {
            if (IsSetup == false)
            {
                OnSetup();
                IsSetup = true;
            }
        }

        protected virtual void OnCleanup()
        {

        }

        protected virtual void OnSetup()
        {
        }
    }
    #endregion
}
