using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenORPG.Common.Dialog;
using OpenORPG.Common.Dialog.Conditions;
using OpenORPG.Database.DAL;
using OpenORPG.Database.Models.ContentTemplates;
using Server.Game.Database;
using Server.Infrastructure.Dialog;

namespace OpenORPG.Toolkit.Views.Content
{
    public partial class DialogEditor : BaseContentForm
    {

        /// <summary>
        /// The root of the tree being rendered for the dialog
        /// </summary>
        private DialogNode _rootDialogNode;

        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto
        };

        public DialogEditor(DialogTemplate template)
        {
            InitializeComponent();
            SetContentTemplate(template);

            // Set reference if it's available
            if (string.IsNullOrEmpty(template.JsonPayload))
                _rootDialogNode = new DialogNode("Default");
            else
                _rootDialogNode = GetFromJson(template.JsonPayload);

            GenerateTree();

            textName.DataBindings.Add("Text", ContentTemplate, "Name");

            // Make sure nodes can be updated
            txtText.LostFocus += TxtTextOnTextChanged;
            txtComment.LostFocus += TxtTextOnTextChanged;
            txtText.GotFocus += TxtTextOnTextChanged;
            txtComment.GotFocus += TxtTextOnTextChanged;
        }

        private void TxtTextOnTextChanged(object sender, EventArgs eventArgs)
        {
            var node = treeDialog.SelectedNode.Tag as IDialogNodeElement;
            treeDialog.SelectedNode.Text = (node.Name + ": " + TruncateLongString(node.Text, 40));
        }

        private DialogNode GetFromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<DialogNode>(json, jsonSerializerSettings);
            }
            catch (Exception exception)
            {
                MessageBox.Show("The payload returned from the server was corrupt.");
                Close();
            }

            return null;
        }

        private string ToJson()
        {
            return JsonConvert.SerializeObject(_rootDialogNode, jsonSerializerSettings);
        }


        private void GenerateTree()
        {
            treeDialog.Nodes.Clear();

            var root = CreateTreeNodeFromDialogNode(_rootDialogNode);
            treeDialog.Nodes.Add(root);

            // Using a recursive approach, we'll generate our tree in the best way we can
            RecurisveAdd(root, _rootDialogNode);
        }

        /// <summary>
        /// Using a recursive approach, generates the hierachy of links and dialog to be navigated
        /// via a tree.
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="node"></param>
        private void RecurisveAdd(TreeNode treeNode, DialogNode node)
        {
            foreach (var childLink in node.Links)
            {
                // Create a link node from the child
                var linkNode = CreateTreeNodeFromDialogLink(childLink);
                treeNode.Nodes.Add(linkNode);

                // If it has a parent, we should go down that rabbit hole, too
                var linkChildNode = childLink.NextNode;
                if (linkChildNode != null)
                {
                    var childTreeNode = CreateTreeNodeFromDialogNode(linkChildNode);
                    linkNode.Nodes.Add(childTreeNode);

                    RecurisveAdd(childTreeNode, linkChildNode);
                }

            }
        }


        private TreeNode CreateTreeNodeFromDialogNode(DialogNode node)
        {
            var treeNode = new TreeNode(node.Name + ": " + TruncateLongString(node.Text, 40));
            treeNode.ImageKey = "script.png";
            treeNode.SelectedImageKey = treeNode.ImageKey;
            treeNode.Tag = node;
            return treeNode;
        }

        private TreeNode CreateTreeNodeFromDialogLink(DialogLink link)
        {
            var treeNode = new TreeNode(link.Name + ": " + TruncateLongString(link.Text, 40));
            treeNode.ImageKey = "link.png";
            treeNode.SelectedImageKey = treeNode.ImageKey;
            treeNode.Tag = link;
            return treeNode;
        }
        private string TruncateLongString(string str, int maxLength)
        {
            var returnStr = str == null ? "" : str.Substring(0, Math.Min(str.Length, maxLength));

            if (str != null && str.Length > maxLength)
                returnStr += "...";

            return returnStr;
        }

        protected override void Save()
        {
            var ContentTemplate = this.ContentTemplate as DialogTemplate;

            // Update the payload
            try
            {
                ContentTemplate.JsonPayload = ToJson();
            }

            catch (Exception exception)
            {
                MessageBox.Show("There was a problem saving the dialog. It was not saved. Aborting...");
                Close();
            }

            using (var db = new GameDatabaseContext())
            {

                var repository = new DialogRepository(db);
                repository.Update(ContentTemplate, ContentTemplate.Id);
            }

            base.Save();
        }

        private void treeDialog_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Perform some binding here to the properties we have
            var dialogElement = e.Node.Tag as IDialogNodeElement;

            if (dialogElement == null)
            {
                MessageBox.Show("The selected element was not of the proper type. This is a bug. Contact a developer");
                return;
            }


            txtComment.DataBindings.Clear();
            txtText.DataBindings.Clear();
            listConditions.DataBindings.Clear();
            textScript.DataBindings.Clear();


            // Bind the elements
            txtComment.DataBindings.Add("Text", dialogElement, "Name");
            txtText.DataBindings.Add("Text", dialogElement, "Text");

            // If we're looking at a link, bind properly and allow editing
            var link = dialogElement as DialogLink;
            if (link != null)
            {
                listConditions.DataSource = link.DialogConditions;
                textScript.DataBindings.Add("Text", link, "Script");
                groupConditions.Enabled = true;
                textScript.Enabled = true;
            }
            else
            {
                textScript.Enabled = false;
                groupConditions.Enabled = false;
            }

        }

        private void addLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = treeDialog.SelectedNode.Tag as DialogNode;

            if (node == null)
            {
                MessageBox.Show("Links can only be placed on dialog nodes. ");
                return;
            }

            // Add a new link to the node
            node.Links.Add(new DialogLink());
            GenerateTree();

        }

        private void addNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var link = treeDialog.SelectedNode.Tag as DialogLink;

            if (link == null)
            {
                MessageBox.Show("Nodes can only be placed on dialog links, with the exception of the root.");
                return;
            }

            // Add a new link to the node
            link.NextNode = new DialogNode();
            GenerateTree();
        }

        private void removeSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var link = treeDialog.SelectedNode.Tag as DialogLink;
            var node = treeDialog.SelectedNode.Tag as DialogNode;

            // We're working with a link
            if (link != null)
            {
                // Just simply erase the next parent
                link.NextNode = null;
            }

            // We're working with a node
            if (node != null)
            {
                throw new NotSupportedException("The deletion of script nodes is not supported yet");
            }

            GenerateTree();
        }

        private void addConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Add condition
            var link = treeDialog.SelectedNode.Tag as DialogLink;

            var dialog = new DialogConditionSelectionForm();
            dialog.ShowDialog();

            //TODO: Something other than minimum level requirement
            if (dialog.Condition != null)
                link.DialogConditions.Add(dialog.Condition);

            UpdateConditionList();
        }

        private void removeConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (listConditions.SelectedIndex < 0)
                return;

            // Remove condition
            var link = treeDialog.SelectedNode.Tag as DialogLink;
            link.DialogConditions.RemoveAt(listConditions.SelectedIndex);

            UpdateConditionList();

        }

        private void UpdateConditionList()
        {
            var link = treeDialog.SelectedNode.Tag as DialogLink;
            listConditions.DataSource = null;
            listConditions.Update();
            if (link != null)
                listConditions.DataSource = link.DialogConditions;
            listConditions.Update();
        }





    }
}
