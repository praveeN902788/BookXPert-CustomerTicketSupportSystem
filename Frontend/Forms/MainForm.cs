using CustomerSupport.Desktop.Models;
using CustomerSupport.Desktop.Services;

namespace CustomerSupport.Desktop.Forms
{
    public partial class MainForm : Form
    {
        private readonly ApiService _apiService;
        private readonly User _currentUser;
        private DataGridView dgvTickets;
        private Button btnCreateTicket;
        private Button btnRefresh;
        private Button btnViewDetails;
        private Label lblWelcome;

        public MainForm(ApiService apiService, User currentUser)
        {
            _apiService = apiService;
            _currentUser = currentUser;
            InitializeComponent();
            LoadTickets();
        }

        private void InitializeComponent()
        {
            this.Text = "Customer Support System - Ticket Management";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Welcome Label
            lblWelcome = new Label
            {
                Text = $"Welcome, {_currentUser.FullName} ({_currentUser.Role})",
                Location = new Point(20, 20),
                Size = new Size(400, 25),
                Font = new Font("Arial", 12, FontStyle.Bold)
            };

            // Create Ticket Button (for Users)
            btnCreateTicket = new Button
            {
                Text = "Create New Ticket",
                Location = new Point(20, 60),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCreateTicket.Click += BtnCreateTicket_Click;

            // Refresh Button
            btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(180, 60),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.Click += BtnRefresh_Click;

            // View Details Button
            btnViewDetails = new Button
            {
                Text = "View Details",
                Location = new Point(290, 60),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnViewDetails.Click += BtnViewDetails_Click;

            // DataGridView for tickets
            dgvTickets = new DataGridView
            {
                Location = new Point(20, 110),
                Size = new Size(940, 420),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false
            };

            // Add columns
            dgvTickets.Columns.Add("TicketNumber", "Ticket #");
            dgvTickets.Columns.Add("Subject", "Subject");
            dgvTickets.Columns.Add("Priority", "Priority");
            dgvTickets.Columns.Add("Status", "Status");
            dgvTickets.Columns.Add("CreatedDate", "Created Date");
            dgvTickets.Columns.Add("AssignedToName", "Assigned To");

            // Set column widths
            dgvTickets.Columns["TicketNumber"].FillWeight = 15;
            dgvTickets.Columns["Subject"].FillWeight = 35;
            dgvTickets.Columns["Priority"].FillWeight = 10;
            dgvTickets.Columns["Status"].FillWeight = 15;
            dgvTickets.Columns["CreatedDate"].FillWeight = 15;
            dgvTickets.Columns["AssignedToName"].FillWeight = 10;

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblWelcome, btnCreateTicket, btnRefresh, btnViewDetails, dgvTickets
            });
        }

        private async void LoadTickets()
        {
            try
            {
                var tickets = await _apiService.GetTicketsAsync();
                dgvTickets.Rows.Clear();

                foreach (var ticket in tickets)
                {
                    var row = new DataGridViewRow();
                    row.CreateCells(dgvTickets);
                    row.Cells[0].Value = ticket.TicketNumber;
                    row.Cells[1].Value = ticket.Subject;
                    row.Cells[2].Value = ticket.Priority.ToString();
                    row.Cells[3].Value = ticket.Status.ToString();
                    row.Cells[4].Value = ticket.CreatedDate.ToString("yyyy-MM-dd HH:mm");
                    row.Cells[5].Value = ticket.AssignedToName ?? "Unassigned";
                    row.Tag = ticket.Id;

                    // Color coding based on priority
                    switch (ticket.Priority)
                    {
                        case TicketPriority.High:
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 235);
                            break;
                        case TicketPriority.Medium:
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);
                            break;
                        case TicketPriority.Low:
                            row.DefaultCellStyle.BackColor = Color.FromArgb(235, 255, 235);
                            break;
                    }

                    dgvTickets.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tickets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCreateTicket_Click(object sender, EventArgs e)
        {
            var createForm = new CreateTicketForm(_apiService);
            if (createForm.ShowDialog() == DialogResult.OK)
            {
                LoadTickets();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadTickets();
        }

        private async void BtnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvTickets.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a ticket to view details.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ticketId = (int)dgvTickets.SelectedRows[0].Tag;
            var ticket = await _apiService.GetTicketAsync(ticketId);

            if (ticket != null)
            {
                var detailsForm = new TicketDetailsForm(_apiService, ticket, _currentUser);
                detailsForm.ShowDialog();
                LoadTickets(); // Refresh in case of updates
            }
            else
            {
                MessageBox.Show("Error loading ticket details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}