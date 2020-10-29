using System.Windows.Controls;

namespace ProductManager.ViewModels
{
    public static class Validation
    {
        public static bool CheckInputTextBoxes(TextBox[] boxes)
        {
            if (!string.IsNullOrWhiteSpace(boxes[0].Text) && !string.IsNullOrWhiteSpace(boxes[1].Text) &&
                !string.IsNullOrWhiteSpace(boxes[2].Text) && !string.IsNullOrWhiteSpace(boxes[3].Text))
                return true;
            else return false;
        }

    }
}
