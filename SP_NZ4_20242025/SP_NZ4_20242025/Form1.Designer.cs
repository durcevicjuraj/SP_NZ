using System.Diagnostics;

namespace SP_NZ4_20242025
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        /// 

        private void PopulatePriorities()
        {
            // Add all available priorities to the list box
            priorityListBox.Items.Add("Idle");
            priorityListBox.Items.Add("Below Normal");
            priorityListBox.Items.Add("Normal");
            priorityListBox.Items.Add("Above Normal");
            priorityListBox.Items.Add("High");
            priorityListBox.Items.Add("Realtime");
        }

        private void RefreshProcesses()
        {
            // Clear the list
            processListBox.Items.Clear();

            try
            {
                // Fetch all running processes
                foreach (var process in Process.GetProcesses())
                {
                    try
                    {
                        string name = process.ProcessName;
                        int pid = process.Id;
                        int priority = (int)process.BasePriority;
                        processListBox.Items.Add($"{name} (PID: {pid}, Prioritet: {priority})");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message); // Ignore processes that throw errors.
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching processes: " + ex.Message);
            }
        }


        private void InitializeComponent()
        {
            processListBox = new ListBox();
            priorityListBox = new ListBox();
            setPriorityButton = new Button();
            refreshButton = new Button();
            SuspendLayout();
            // 
            // processListBox
            // 
            processListBox.ItemHeight = 15;
            processListBox.Location = new Point(30, 30);
            processListBox.Name = "processListBox";
            processListBox.Size = new Size(350, 289);
            processListBox.TabIndex = 0;
            // 
            // priorityListBox
            // 
            priorityListBox.ItemHeight = 15;
            priorityListBox.Location = new Point(402, 30);
            priorityListBox.Name = "priorityListBox";
            priorityListBox.Size = new Size(200, 109);
            priorityListBox.TabIndex = 1;
            priorityListBox.SelectedIndexChanged += priorityListBox_SelectedIndexChanged;
            // 
            // setPriorityButton
            // 
            setPriorityButton.Location = new Point(451, 145);
            setPriorityButton.Name = "setPriorityButton";
            setPriorityButton.Size = new Size(100, 30);
            setPriorityButton.TabIndex = 2;
            setPriorityButton.Text = "Postavi Prioritet";
            setPriorityButton.Click += setPriorityButton_Click;
            // 
            // refreshButton
            // 
            refreshButton.Location = new Point(153, 325);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(100, 30);
            refreshButton.TabIndex = 3;
            refreshButton.Text = "Osvježi";
            refreshButton.Click += refreshButton_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(650, 400);
            Controls.Add(processListBox);
            Controls.Add(priorityListBox);
            Controls.Add(setPriorityButton);
            Controls.Add(refreshButton);
            Name = "Form1";
            Text = "Process Manager";
            ResumeLayout(false);
        }


        #endregion

        private ListBox processListBox;
        private ListBox priorityListBox;
        private Button refreshButton;
        private Button setPriorityButton;
    }
}
