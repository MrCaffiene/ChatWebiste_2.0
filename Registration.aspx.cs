using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace ChatWebiste_2._0
{
    public partial class Registration : System.Web.UI.Page
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ChatAppDBConnection"].ToString();

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // Validate that the fields are not empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                lblError.Text = "All fields are required.";
                return;
            }

            // Validate that passwords match
            if (password != confirmPassword)
            {
                lblError.Text = "Passwords do not match.";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    // Check if the username already exists
                    string checkQuery = "SELECT COUNT(1) FROM Users WHERE Username = @Username";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Username", username);

                    conn.Open();
                    int existingUser = (int)checkCmd.ExecuteScalar();
                    conn.Close();

                    if (existingUser > 0)
                    {
                        lblError.Text = "Username is already taken.";
                        return;
                    }

                    // If not, insert the new user
                    string insertQuery = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @Password)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@Password", password); // You should hash the password here in production

                    conn.Open();
                    insertCmd.ExecuteNonQuery();
                    conn.Close();

                    // Redirect to login page after successful registration
                    Response.Redirect("Login.aspx");
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
            }
        }

        protected void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
