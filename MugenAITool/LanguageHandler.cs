using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MugenAITool
{
    class LanguageHandler
    {
        private void ApplyResourcesToolStripMenuItem(ComponentResourceManager resources, ToolStripMenuItem ParentTsmi)
        {
            if (ParentTsmi is ToolStripMenuItem)
            {
                resources.ApplyResources(ParentTsmi, ParentTsmi.Name);
                if (ParentTsmi.DropDownItems.Count > 0)
                {
                    foreach (ToolStripMenuItem Tsmi in ParentTsmi.DropDownItems)
                    {
                        ApplyResourcesToolStripMenuItem(resources, Tsmi);
                    }
                }
            }
        }

        public void ApplyResourcesControl(ComponentResourceManager resources, Control ParentCtl)
        {
            resources.ApplyResources(ParentCtl, ParentCtl.Name);
            foreach (Control Ctl in ParentCtl.Controls)
            {
                resources.ApplyResources(Ctl, Ctl.Name);
                if (Ctl is TextBox) continue;
                if (Ctl is MenuStrip ms)
                {
                    if (ms.Items.Count > 0)
                    {
                        foreach (ToolStripMenuItem Tsmi in ms.Items)
                        {
                            ApplyResourcesToolStripMenuItem(resources, Tsmi);
                        }
                    }
                }
                else
                {
                    ApplyResourcesControl(resources, Ctl);
                }
            }
        }

        public void ApplyResourcesCheckedListBox(ComponentResourceManager resources, CheckedListBox Clb)
        {
            // Record the status of items in checked list box
            int CheckedFlag = 0;
            for (int i = 0; i < Clb.Items.Count; i += 1)
            {
                if (Clb.GetItemChecked(i)) CheckedFlag += (int)Math.Pow(2, i);
            }

            // Clear the checked list box and refill it after translation
            Clb.Items.Clear();
            Clb.Items.AddRange(
                new object[] {
                    resources.GetString("Mainpage_checkedList.Items"),
                    resources.GetString("Mainpage_checkedList.Items1"),
                    resources.GetString("Mainpage_checkedList.Items2"),
                    resources.GetString("Mainpage_checkedList.Items3"),
                    resources.GetString("Mainpage_checkedList.Items4"),
                    resources.GetString("Mainpage_checkedList.Items5"),
                    resources.GetString("Mainpage_checkedList.Items6"),
                    resources.GetString("Mainpage_checkedList.Items7"),
                    resources.GetString("Mainpage_checkedList.Items8")
                });

            // Recover the status of items in checked list box
            for (int i = 0; i < Clb.Items.Count; i += 1)
            {
                Clb.SetItemChecked(i, (CheckedFlag % 2 == 1));
                CheckedFlag /= 2;
            }
        }
    }
}
