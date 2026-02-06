using CustomerSupport.Desktop.Models;
using CustomerSupport.Desktop.Services;

namespace CustomerSupport.Desktop.Forms
{
    public partial class TicketDetailsForm : Form
    {
        private readonly ApiService _apiService;
        private readonly Ticket _ticket;
        private readonly User _currentUser;
        private bool _isAdmin;
        private Label lblTicketNumber;
        private Label lblSubject;
        private TextBox txtDescription;
        private Label lblPriority;
        private Label lblStatus;
        private Label lblCreatedDate;
        private Label lblAssignedTo;
        private ComboBox cboAssignTo;
        private ComboBox cboStatus;
        private TextBox txtComment;
        private CheckBox chkInternalComment;
        private Button btnSave;
        private DataGridView dgvComments;
        private DataGridView dgvHistory;

        public TicketDetailsForm(ApiService apiService, Ticket ticket, User currentUser)
        {
            _apiService = apiService;
            _ticket = ticket;
            _currentUser = currentUser;
            _isAdmin = currentUser.Role == UserRole.Admin;
            InitializeComponent();
            LoadTicketDetails();
        }

        private void InitializeComponent()
        {
            this.Text = $"Ticket Details - {_ticket.TicketNumber}";
            this.Size = new Size(800, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            var lblTicketNumberLabel = new Label
            {
                Text = "Ticket Number:",
                Location = new Point(20, 20),
                Size = new Size(100, 23),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            lblTicketNumber = new Label
            {
                Text = _ticket.TicketNumber,
                Location = new Point(130, 20),
                Size = new Size(200, 23)
            };
            var lblSubjectLabel = new Label
            {
                Text = "Subject:",
                Location = new Point(20, 50),
                Size = new Size(100, 23),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            lblSubject = new Label
            {
                Text = _ticket.Subject,
                Location = new Point(130, 50),
                Size = new Size(400, 23)
            };
            var lblDescriptionLabel = new Label
            {
                Text = "Description:",
                Location = new Point(20, 80),
                Size = new Size(100, 23),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            txtDescription = new TextBox
            {
                Text = _ticket.Description,
                Location = new Point(130, 80),
                Size = new Size(400, 60),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };
            var lblPriorityLabel = new Label
            {
                Text = "Priority:",
                Location = new Point(20, 160),
                Size = new Size(60, 23),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            lblPriority = new Label
            {
                Text = _ticket.Priority.ToString(),
                Location = new Point(90, 160),
                Size = new Size(80, 23)
            };

            var lblStatusLabel = new Label
            {
                Text = "Status:",
                Location = new Point(180, 160),
                Size = new Size(50, 23),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            lblStatus = new Label
            {
                Text = _ticket.Status.ToString(),
                Location = new Point(240, 160),
                Size = new Size(80, 23)
            };

            var lblCreatedDateLabel = new Label
            {
                Text = "Created:",
                Location = new Point(330, 160),
                Size = new Size(60, 23),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            lblCreatedDate = new Label
            {
                Text = _ticket.CreatedDate.ToString("yyyy-MM-dd HH:mm"),
                Location = new Point(400, 160),
                Size = new Size(130, 23)
            };

            var lblAssignedToLabel = new Label
            {
                Text = "Assigned To:",
                Location = new Point(20, 190),
                Size = new Size(100, 23),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            lblAssignedTo = new Label
            {
                Text = _ticket.AssignedToName ?? "Unassigned",
                Location = new Point(130, 190),
                Size = new Size(200, 23)
            };

            // Admin Controls
            if (_isAdmin)
            {
                var lblAssignToLabel = new Label
                {
                    Text = "Assign To:",
                    Location = new Point(20, 230),
                    Size = new Size(80, 23),
                    Font = new Font("Arial", 9, FontStyle.Bold)
                };

                cboAssignTo = new ComboBox
                {
                    Location = new Point(110, 230),
                    Size = new Size(150, 23),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                var lblStatusChangeLabel = new Label
                {
                    Text = "Change Status:",
                    Location = new Point(280, 230),
                    Size = new Size(100, 23),
                    Font = new Font("Arial", 9, FontStyle.Bold)
                };

                cboStatus = new ComboBox
                {
                    Location = new Point(390, 230),
                    Size = new Size(120, 23),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboStatus.Items.AddRange(new[] { "Open", "InProgress", "Closed" });

                this.Controls.AddRange(new Control[] { lblAssignToLabel, cboAssignTo, lblStatusChangeLabel, cboStatus });
            }
            var lblCommentLabel = new Label
            {
                Text = "Add Comment:",
                Location = new Point(20, _isAdmin ? 270 : 230),
                Size = new Size(100, 23),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            txtComment = new TextBox
            {
                Location = new Point(130, _isAdmin ? 270 : 230),
                Size = new Size(400, 60),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            if (_isAdmin)
            {
                chkInternalComment = new CheckBox
                {
                    Text = "Internal Comment",
                    Location = new Point(130, _isAdmin ? 340 : 300),
                    Size = new Size(150, 23)
                };
            }
            btnSave = new Button
            {
                Text = "Save Changes",
                Location = new Point(450, _isAdmin ? 340 : 300),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;

            var lblCommentsLabel = new Label
            {
                Text = "Comments:",
                Location = new Point(20, _isAdmin ? 390 : 350),
                Size = new Size(100, 23),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            dgvComments = new DataGridView
            {
                Location = new Point(20, _isAdmin ? 420 : 380),
                Size = new Size(350, 150),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false
            };

            var lblHistoryLabel = new Label
            {
                Text = "Status History:",
                Location = new Point(390, _isAdmin ? 390 : 350),
                Size = new Size(100, 23),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            dgvHistory = new DataGridView
            {
                Location = new Point(390, _isAdmin ? 420 : 380),
                Size = new Size(350, 150),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false
            };
            var controls = new List<Control> {
                lblTicketNumberLabel, lblTicketNumber, lblSubjectLabel, lblSubject,
                lblDescriptionLabel, txtDescription, lblPriorityLabel, lblPriority,
                lblStatusLabel, lblStatus, lblCreatedDateLabel, lblCreatedDate,
                lblAssignedToLabel, lblAssignedTo, lblCommentLabel, txtComment,
                btnSave, lblCommentsLabel, dgvComments, lblHistoryLabel, dgvHistory
            };

            if (_isAdmin && chkInternalComment != null)
            {
                controls.Add(chkInternalComment);
            }

            this.Controls.AddRange(controls.ToArray());
        }

        private async void LoadTicketDetails()
        {
            if (_isAdmin && cboAssignTo != null)
            {
                var adminUsers = await _apiService.GetAdminUsersAsync();
                cboAssignTo.Items.Clear();
                cboAssignTo.Items.Add("Unassigned");
                
                foreach (var admin in adminUsers)
                {
                    cboAssignTo.Items.Add(admin.FullName);
                    if (_ticket.AssignedToId == admin.Id)
                    {
                        cboAssignTo.SelectedIndex = cboAssignTo.Items.Count - 1;
                    }
                }

                if (cboAssignTo.SelectedIndex == -1)
                {
                    cboAssignTo.SelectedIndex = 0; 
                }
                if (cboStatus != null)
                {
                    cboStatus.SelectedItem = _ticket.Status.ToString();
                }
            }
            LoadComments();
            LoadHistory();
        }

        private void LoadComments()
        {
            dgvComments.Columns.Clear();
            dgvComments.Columns.Add("CreatedByName", "By");
            dgvComments.Columns.Add("CommentText", "Comment");
            dgvComments.Columns.Add("CreatedDate", "Date");
            dgvComments.Columns.Add("IsInternal", "Internal");

            dgvComments.Rows.Clear();
            foreach (var comment in _ticket.Comments)
            {
                var row = new DataGridViewRow();
                row.CreateCells(dgvComments);
                row.Cells[0].Value = comment.CreatedByName;
                row.Cells[1].Value = comment.CommentText;
                row.Cells[2].Value = comment.CreatedDate.ToString("yyyy-MM-dd HH:mm");
                row.Cells[3].Value = comment.IsInternal ? "Yes" : "No";

                if (comment.IsInternal)
                {
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
                }

                dgvComments.Rows.Add(row);
            }
        }

        private void LoadHistory()
        {
            dgvHistory.Columns.Clear();
            dgvHistory.Columns.Add("ChangedByName", "Changed By");
            dgvHistory.Columns.Add("StatusChange", "Status Change");
            dgvHistory.Columns.Add("ChangeDate", "Date");

            dgvHistory.Rows.Clear();
            foreach (var history in _ticket.StatusHistory)
            {
                var row = new DataGridViewRow();
                row.CreateCells(dgvHistory);
                row.Cells[0].Value = history.ChangedByName;
                row.Cells[1].Value = $"{history.OldStatus} → {history.NewStatus}";
                row.Cells[2].Value = history.ChangeDate.ToString("yyyy-MM-dd HH:mm");

                dgvHistory.Rows.Add(row);
            }
        }

        //private async void BtnSave_Click(object sender, EventArgs e)
        //{
        //    btnSave.Enabled = false;
        //    btnSave.Text = "Saving...";

        //    try
        //    {
        //        var updateRequest = new
        //        {
        //            AssignedToId = _isAdmin && cboAssignTo != null && cboAssignTo.SelectedIndex > 0 ? 
        //                GetSelectedAdminId() : (int?)null,
        //            Status = _isAdmin && cboStatus != null && cboStatus.SelectedItem != null ? 
        //                Enum.Parse<TicketStatus>(cboStatus.SelectedItem.ToString()) : (TicketStatus?)null,
        //            Comment = !string.IsNullOrWhiteSpace(txtComment.Text) ? txtComment.Text.Trim() : null,
        //            IsInternalComment = _isAdmin && chkInternalComment?.Checked == true
        //        };

        //        var success = await _apiService.UpdateTicketAsync(_ticket.Id, updateRequest);

        //        if (success)
        //        {
        //            MessageBox.Show("Ticket updated successfully!", "Success", 
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            // Refresh ticket details
        //            var updatedTicket = await _apiService.GetTicketAsync(_ticket.Id);
        //            if (updatedTicket != null)
        //            {
        //                // Update the form with new data
        //                lblStatus.Text = updatedTicket.Status.ToString();
        //                lblAssignedTo.Text = updatedTicket.AssignedToName ?? "Unassigned";
        //                _ticket.Comments = updatedTicket.Comments;
        //                _ticket.StatusHistory = updatedTicket.StatusHistory;
        //                LoadComments();
        //                LoadHistory();
        //            }

        //            txtComment.Clear();
        //            if (chkInternalComment != null)
        //                chkInternalComment.Checked = false;
        //        }
        //        else
        //        {
        //            MessageBox.Show("Failed to update ticket.", "Error", 
        //                MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error updating ticket: {ex.Message}", "Error", 
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        btnSave.Enabled = true;
        //        btnSave.Text = "Save Changes";
        //    }
        //}

        //private async int? GetSelectedAdminId()
        //{
        //    if (cboAssignTo == null || cboAssignTo.SelectedIndex <= 0)
        //        return null;

        //    var adminUsers = await _apiService.GetAdminUsersAsync();
        //    var selectedName = cboAssignTo.SelectedItem.ToString();
        //    var admin = adminUsers.FirstOrDefault(a => a.FullName == selectedName);
        //    return admin?.Id;
        //}



        private async void BtnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            btnSave.Text = "Saving...";

            try
            {
                int? assignedAdminId = null;

                if (_isAdmin && cboAssignTo != null && cboAssignTo.SelectedIndex > 0)
                {
                    assignedAdminId = await GetSelectedAdminId();
                }

                var updateRequest = new
                {
                    AssignedToId = assignedAdminId,

                    Status = _isAdmin && cboStatus?.SelectedItem != null
                        ? Enum.Parse<TicketStatus>(cboStatus.SelectedItem.ToString())
                        : (TicketStatus?)null,

                    Comment = !string.IsNullOrWhiteSpace(txtComment.Text)
                        ? txtComment.Text.Trim()
                        : null,

                    IsInternalComment = _isAdmin && chkInternalComment?.Checked == true
                };

                var success = await _apiService.UpdateTicketAsync(_ticket.Id, updateRequest);

                if (success)
                {
                    MessageBox.Show(
                        "Ticket updated successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    var updatedTicket = await _apiService.GetTicketAsync(_ticket.Id);
                    if (updatedTicket != null)
                    {
                        lblStatus.Text = updatedTicket.Status.ToString();
                        lblAssignedTo.Text = updatedTicket.AssignedToName ?? "Unassigned";

                        _ticket.Comments = updatedTicket.Comments;
                        _ticket.StatusHistory = updatedTicket.StatusHistory;

                        LoadComments();
                        LoadHistory();
                    }

                    txtComment.Clear();
                    if (chkInternalComment != null)
                        chkInternalComment.Checked = false;
                }
                else
                {
                    MessageBox.Show(
                        "Failed to update ticket.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error updating ticket: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                btnSave.Enabled = true;
                btnSave.Text = "Save Changes";
            }
        }

        private async Task<int?> GetSelectedAdminId()
        {
            if (cboAssignTo == null || cboAssignTo.SelectedIndex <= 0)
                return null;

            var adminUsers = await _apiService.GetAdminUsersAsync();

            var selectedName = cboAssignTo.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedName))
                return null;

            var admin = adminUsers
                .FirstOrDefault(a => a.FullName == selectedName);

            return admin?.Id;
        }

    }
}