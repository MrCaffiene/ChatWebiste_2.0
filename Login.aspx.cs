using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace ChatWebiste_2._0
{
    public partial class Login : System.Web.UI.Page
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ChatAppDBConnection"].ToString();

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Please enter both username and password.";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND PasswordHash = @Password";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password); // You should hash passwords in a real app

                    conn.Open();
                    int userCount = (int)cmd.ExecuteScalar();
                    conn.Close();

                    if (userCount > 0)
                    {
                        // Store session and redirect to the chat page
                        Session["Username"] = username;
                        Response.Redirect("Chat.aspx");
                    }
                    else
                    {
                        lblError.Text = "Invalid username or password.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
            }
        }
    }
}
