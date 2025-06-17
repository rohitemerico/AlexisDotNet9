namespace Dashboard.Entities
{
    public enum CRUD_action
    {
        Create,
        Update
    }

    public class CRUD
    {
        const string BtnCreateText = "Confirm";
        const string BtnUpdateText = "Update";

        /// <summary>
        /// Return the text correlated to the action (e.g. update action will return 'update').
        /// Change const string to customize text. Mainly to update button text (create <--> update).
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetButtonText(CRUD_action action)
        {
            string buttonText = "";

            switch (action)
            {
                case CRUD_action.Create:
                    buttonText = BtnCreateText;
                    break;
                case CRUD_action.Update:
                    buttonText = BtnUpdateText;
                    break;
                default:
                    buttonText = "";
                    break;
            }

            return buttonText;
        }
    }
}
