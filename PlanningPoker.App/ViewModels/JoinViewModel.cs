namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Diagnostics;

    /*
     * Waiting for api until implementation
     * what you will see is pseudocode
     */
    public class JoinViewModel
    {
        private string testKey = "1234567";

        public JoinViewModel()
        {
        }

        /*
         * Returns allowance
         */
        public void JoinLobby(string key)
        {
            Debug.WriteLine("Connection!");

            // Connect to database and find session
        }

        private bool KeyExist(string key)
        {
            // Call the api to see if key exist and return
            return (key == this.testKey) ? true : false;
        }
    }
}
