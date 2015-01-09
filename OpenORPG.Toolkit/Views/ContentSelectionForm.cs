using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenORPG.Toolkit.Content;
using Server.Game.Database.Models.ContentTemplates;

namespace OpenORPG.Toolkit.Views
{
    public partial class ContentSelectionForm : Form
    {
        private Type _contentType;

        public ContentSelectionForm(Type contentType)
        {
            _contentType = contentType;
            InitializeComponent();

            RefreshTree();
        }

        private void RefreshTree()
        {

            treeView1.Nodes.Clear();
            
            // Bind
            var templates = GetContentTemplates();

            // An anonymous function for generating TreeNodes based on the given path
            Func<
                IEnumerable<IEnumerable<string>>,
                IEnumerable<TreeNode>>
                buildTreeNode = null;

            buildTreeNode = xss =>
                xss
                    .ToLookup(xs => xs.FirstOrDefault(), xs => xs.Skip(1))
                    .Where(xs => xs.Key != null)
                    .Select(xs => new TreeNode(xs.Key, buildTreeNode(xs).ToArray()));


            var lines = GetUniqueContentCategories(templates);

            var tree =
                buildTreeNode(lines
                    .Select(x => new[] {"Content",}.Concat(x.Split('/').Skip(1))));

            foreach (var node in tree)
                treeView1.Nodes.Add(node);

            // This is a safety in case there's no nodes within the virtual categories
            if (treeView1.Nodes.Count == 0)
                treeView1.Nodes.Add(new TreeNode("Content"));

            AddContentTemplateNodes(templates);

            //treeView1.ExpandAll();
        }

        /// <summary>
        /// Given a set of content templates, is capable of returning their 
        /// </summary>
        /// <param name="contentTemplates"></param>
        /// <returns></returns>
        private IEnumerable<string> GetUniqueContentCategories(IEnumerable<IContentTemplate> contentTemplates)
        {
            var categorySet = new HashSet<string>();

            foreach (var template in contentTemplates)
            {
                if (!string.IsNullOrEmpty(template.VirtualCategory))
                    categorySet.Add(template.VirtualCategory);
            }

            return categorySet.AsEnumerable();
        }

        /// <summary>
        /// Given a set of templates, traverses the tree listing for nodes that match up to the virtual category.
        /// Then, they are created, setup and placed as a child of this parent node.
        /// </summary>
        /// <param name="contentTemplates"></param>
        private void AddContentTemplateNodes(IEnumerable<IContentTemplate> contentTemplates)
        {

            foreach (var template in contentTemplates)
            {

                var connectedNode = GetNodeFromPath(treeView1.Nodes[0], template.VirtualCategory);

                // Just go to the root if we can't find anywhere for you
                if (connectedNode == null)
                    connectedNode = treeView1.Nodes[0];

                var newNode = new TreeNode(template.Name);
                newNode.ImageKey = "page_lightning.png";
                newNode.SelectedImageKey = "page_lightning.png";
                newNode.Tag = template;
          
                connectedNode.Nodes.Add(newNode);
            }

        }

        private TreeNode GetNodeFromPath(TreeNode root, string path)
        {
            if (path == null)
                return null;

            path = path.Replace("/", "\\");
            TreeNode foundNode = null;
            foreach (TreeNode tn in root.Nodes)
            {
                var searchText = tn.FullPath.TrimStart(("Content").ToCharArray());
                if (searchText == path)
                {
                    return tn;
                }
                else if (tn.Nodes.Count > 0)
                {
                    foundNode = GetNodeFromPath(tn, path);
                }
                if (foundNode != null)
                    return foundNode;
            }
            return null;
        }

        private string GetNodePathWithoutContent(TreeNode node)
        {
            return node.FullPath.TrimStart(("Content").ToCharArray());
        }

        private List<IContentTemplate> GetContentTemplates()
        {
            return ContentTypeResolver.GetContentTemplateFromType(_contentType);
        }



        public IContentTemplate SelectedTemplate { get; private set; }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var item = GetTemplateFromNode();

            if (item != null)
            {
                SelectedTemplate = item;
                Close();
            }
            else
            {
                // Do something here if we want to act on a folder node, for now there's no need.
            }

        
        }

        private IContentTemplate GetTemplateFromNode()
        {
            if (treeView1.SelectedNode == null)
                return null;

            return (IContentTemplate) treeView1.SelectedNode.Tag;
        }

        private void contextAddFolder_Click(object sender, EventArgs e)
        {
            var parentNode = GetNearestFolderNode();

            if (parentNode == null)
                return;

            string newName = "";
            var result = InputHelper.ShowInputDialog(ref newName);

            if (!string.IsNullOrEmpty(newName) && result == DialogResult.OK)
            {
                var newFolder = new TreeNode(newName);
                parentNode.Nodes.Add(newFolder);
            }
        }


        private void contextAddContent_Click(object sender, EventArgs e)
        {
            var parentNode = GetNearestFolderNode();
            if (parentNode == null)
                return;
            ContentTypeResolver.AddContentWithVirtualCategory(_contentType, GetNodePathWithoutContent(parentNode));
            RefreshTree();
        }

        private TreeNode GetNearestFolderNode()
        {
            var template = GetTemplateFromNode();
            var parentNode = treeView1.SelectedNode;

            // If it's a content, go searching for the parent node.
            if (template != null)
                parentNode = parentNode.Parent;
            return parentNode;
        }

     


    }
}
