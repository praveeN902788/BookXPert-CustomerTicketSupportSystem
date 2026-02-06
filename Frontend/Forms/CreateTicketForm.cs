using CustomerSupport.Desktop.Models;
using CustomerSupport.Desktop.Services;

namespace CustomerSupport.Desktop.Forms
{
    public partial class CreateTicketForm : Form
    {
        private readonly ApiService _apiService;
        private TextBox txtSubject;
        private TextBox txtDescription;
        private ComboBox cboPriority;
        private Button btnSubmit;
        private Button btnCancel;

        public CreateTicketForm(ApiService apiService)
        {
            _apiService = apiService;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Create New Ticket";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            var lblSubject = new Label
            {
                Text = "Subject:",
                Location = new Point(20, 30),
                Size = new Size(80, 23)
            };

            txtSubject = new TextBox
            {
                Location = new Point(110, 30),
                Size = new Size(350, 23)
            };
            var lblDescription = new Label
            {
                Text = "Description:",
                Location = new Point(20, 70),
                Size = new Size(80, 23)
            };

            txtDescription = new TextBox
            {
                Location = new Point(110, 70),
                Size = new Size(350, 150),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            var lblPriority = new Label
            {
                Text = "Priority:",
                Location = new Point(20, 240),
                Size = new Size(80, 23)
            };

            cboPriority = new ComboBox
            {
                Location = new Point(110, 240),
                Size = new Size(150, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboPriority.Items.AddRange(new[] { "Low", "Medium", "High" });
            cboPriority.SelectedIndex = 1; 
            btnSubmit = new Button
            {
                Text = "Submit Ticket",
                Location = new Point(280, 300),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSubmit.Click += BtnSubmit_Click;

            // Cancel Button
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(150, 300),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            this.Controls.AddRange(new Control[] {
                lblSubject, txtSubject, lblDescription, txtDescription,
                lblPriority, cboPriority, btnSubmit, btnCancel
            });
        }

        private async void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSubject.Text))
            {
                MessageBox.Show("Please enter a subject.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSubmit.Enabled = false;
            btnSubmit.Text = "Creating...";

            try
            {
                var request = new CreateTicketRequest
                {
                    Subject = txtSubject.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Priority = (TicketPriority)cboPriority.SelectedIndex
                };

                var ticket = await _apiService.CreateTicketAsync(request);

                if (ticket != null)
                {
                    MessageBox.Show($"Ticket created successfully!\nTicket Number: {ticket.TicketNumber}", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Failed to create ticket. Please try again.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating ticket: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSubmit.Enabled = true;
                btnSubmit.Text = "Submit Ticket";
            }
        }
    }
}