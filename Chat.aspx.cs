using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using WebGrease.Activities;

namespace ChatWebsite_2._0
{
    public partial class Chat : System.Web.UI.Page
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ChatAppDBConnection"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is logged in (session should contain the username)
            if (Session["Username"] == null)
            {
                // Redirect to the login page if not logged in
                Response.Redirect("Login.aspx");
            }

            // Load messages only on the first page load (not during postbacks)
            if (!IsPostBack)
            {
                LoadMessages();
            }
        }

        // Load chat messages from the database
        private void LoadMessages()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT m.MessageText, m.Timestamp, u.Username FROM Messages m JOIN Users u ON m.UserId = u.UserId ORDER BY m.Timestamp ASC";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    da.Fill(dt);
                    rptMessages.DataSource = dt;
                    rptMessages.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error accordingly
                ShowErrorMessage("Error loading messages: " + ex.Message);
            }
        }

        // Send a message to the database
        protected void btnSend_Click(object sender, EventArgs e)
        {
            string username = Session["Username"].ToString();
            string messageText = txtMessage.Value.Trim(); // Use 'Value' property for HtmlTextArea

            // Ensure that the message is not empty
            if (!string.IsNullOrEmpty(messageText))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        // Parameterized query to prevent SQL injection
                        string query = "INSERT INTO Messages (UserId, MessageText) " +
                                       "SELECT UserId, @MessageText FROM Users WHERE Username = @Username";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@MessageText", messageText);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    // Clear the message input field after sending
                    txtMessage.Value = string.Empty; // Use 'Value' property to clear

                    // Reload messages after sending
                    LoadMessages();
                }
                catch (Exception ex)
                {
                    // Log or handle the error accordingly
                    ShowErrorMessage("Error sending message: " + ex.Message);
                }
            }
            else
            {
                // Inform the user if the message is empty
                ShowErrorMessage("Message cannot be empty.");
            }
        }

        // Function to display error messages on the page
        private void ShowErrorMessage(string message)
        {
            lblError.Text = message;  // Assuming you have a Label control with ID "lblError" to display errors
            lblError.Visible = true;
        }

        // Logout button click event
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear the session (this will log out the user)
            Session.Clear();  // Clear all session data
            Session.Abandon();  // End the session

            // Redirect to the Login page after logout
            Response.Redirect("Login.aspx");
        }
    }
}
