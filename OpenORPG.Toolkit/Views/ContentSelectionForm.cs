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

            // Bind
            var templates = GetContentTemplates();
            listContent.DataSource = templates;

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
                    .Select(x => new[] { "Content", }.Concat(x.Split('/').Skip(1))));

            foreach (var node in tree)
                treeView1.Nodes.Add(node);

            AddContentTemplateNodes(templates);

            treeView1.ExpandAll();
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

        private List<IContentTemplate> GetContentTemplates()
        {
            return ContentTypeResolver.GetContentTemplateFromType(_contentType);
        }

        private void listContent_DoubleClick(object sender, EventArgs e)
        {
            var item = (IContentTemplate)listContent.SelectedItem;
            SelectedTemplate = item;
            Close();
        }

        public IContentTemplate SelectedTemplate { get; private set; }


    }
}
