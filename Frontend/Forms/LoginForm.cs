using CustomerSupport.Desktop.Models;
using CustomerSupport.Desktop.Services;

namespace CustomerSupport.Desktop.Forms
{
    public partial class LoginForm : Form
    {
        private readonly ApiService _apiService;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblMessage;

        public LoginForm(ApiService apiService)
        {
            _apiService = new ApiService("https://localhost:57230");
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Customer Support - Login";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            var lblTitle = new Label
            {
                Text = "Customer Support System",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(50, 30),
                Size = new Size(300, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            var lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(50, 80),
                Size = new Size(80, 23)
            };

            txtUsername = new TextBox
            {
                Location = new Point(140, 80),
                Size = new Size(200, 23)
            };
            var lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(50, 120),
                Size = new Size(80, 23)
            };

            txtPassword = new TextBox
            {
                Location = new Point(140, 120),
                Size = new Size(200, 23),
                UseSystemPasswordChar = true
            };
            btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(140, 160),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.Click += BtnLogin_Click;
            lblMessage = new Label
            {
                Location = new Point(50, 200),
                Size = new Size(300, 40),
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.AddRange(new Control[] {
                lblTitle, lblUsername, txtUsername, lblPassword, txtPassword, btnLogin, lblMessage
            });
            txtUsername.Text = "admin";
            txtPassword.Text = "admin123";
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMessage.Text = "Please enter username and password";
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Logging in...";
            lblMessage.Text = "";

            try
            {
                var loginRequest = new LoginRequest
                {
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Text
                };

                var response = await _apiService.LoginAsync(loginRequest);

                if (response.Success && response.User != null)
                {
                    _apiService.SetAuthToken(response.Token);
                    
                    this.Hide();
                    var mainForm = new MainForm(_apiService, response.User);
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    lblMessage.Text = response.Message;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Login failed: {ex.Message}";
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Login";
            }
        }
    }
}