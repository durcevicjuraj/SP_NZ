using System.Diagnostics;

namespace SP_NZ4_20242025
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RefreshProcesses();
            PopulatePriorities();
        }


        private void processListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (processListBox.SelectedItem != null)
            {
                MessageBox.Show($"Selected Process: {processListBox.SelectedItem.ToString()}");
            }
        }


        private void refreshButton_Click(object sender, EventArgs e)
        {
            RefreshProcesses();  // Call RefreshProcesses to update the process list
        }


        private void setPriorityButton_Click(object sender, EventArgs e)
        {
            // Ensure a process and priority are selected
            if (processListBox.SelectedItem == null || priorityListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a process and a priority.");
                return;
            }

            // Extract the selected process ID from the selected item
            string selectedProcess = processListBox.SelectedItem.ToString();
            int pidStartIndex = selectedProcess.IndexOf("PID:") + 4;
            int pidEndIndex = selectedProcess.IndexOf(",", pidStartIndex);
            int pid = int.Parse(selectedProcess.Substring(pidStartIndex, pidEndIndex - pidStartIndex).Trim());

            // Get the selected priority
            string selectedPriority = priorityListBox.SelectedItem.ToString();

            try
            {
                // Find the process by ID
                Process process = Process.GetProcessById(pid);

                // Set the priority class based on the selected priority
                switch (selectedPriority)
                {
                    case "Idle":
                        process.PriorityClass = ProcessPriorityClass.Idle;
                        break;
                    case "Below Normal":
                        process.PriorityClass = ProcessPriorityClass.BelowNormal;
                        break;
                    case "Normal":
                        process.PriorityClass = ProcessPriorityClass.Normal;
                        break;
                    case "Above Normal":
                        process.PriorityClass = ProcessPriorityClass.AboveNormal;
                        break;
                    case "High":
                        process.PriorityClass = ProcessPriorityClass.High;
                        break;
                    case "Realtime":
                        process.PriorityClass = ProcessPriorityClass.RealTime;
                        break;
                    default:
                        throw new InvalidOperationException("Invalid priority selected.");
                }

                MessageBox.Show($"Priority of process '{process.ProcessName}' changed to {selectedPriority}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to set priority: {ex.Message}");
            }
        }


        private void priorityListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
