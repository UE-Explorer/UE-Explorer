using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UELib;
using UELib.Annotations;
using UELib.Core;

namespace UEExplorer.UI.Nodes
{
    public class ObjectTreeBuilder : ObjectVisitor<TreeNode[]>
    {
        [CanBeNull]
        public override TreeNode[] Visit(IAcceptable visitor)
        {
            var memberNodes = VisitMembers(visitor);
            var visitorNodes = base.Visit(visitor);
            if (memberNodes == null)
            {
                return visitorNodes;
            }

            return visitorNodes != null
                ? visitorNodes.Concat(memberNodes).ToArray()
                : memberNodes.ToArray();
        }

        public List<TreeNode> VisitMembers(object visitable)
        {
            var subNodes = new List<TreeNode>();
            var members = visitable.GetType().GetMembers();
            foreach (var member in members)
            {
                switch (member)
                {
                    case FieldInfo field when visitable is IUnrealSerializableClass:
                        {
                            var type = field.FieldType;
                            if (type == typeof(UArray<UObject>))
                            {
                                var value = (UArray<UObject>)field.GetValue(visitable);
                                if (value == null) continue;

                                var attr = field.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>();
                                var memberNode = new TreeNode(attr != null ? attr.DisplayName : field.Name);
                                foreach (var obj in value)
                                {
                                    memberNode.Nodes.Add(new ObjectNode(obj));
                                }

                                subNodes.Add(memberNode);
                            }
                            else if (type.IsSubclassOf(typeof(UObject)))
                            {
                                var value = (UObject)field.GetValue(visitable);
                                if (value == null) continue;
                                var attr = field.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>();
                                var memberNode = new TreeNode(attr != null ? attr.DisplayName : field.Name)
                                {
                                    Tag = value
                                };
                                memberNode.Nodes.Add(new ObjectNode(value));
                                subNodes.Add(memberNode);
                            }
                            else
                            {
                                var value = field.GetValue(visitable);
                                var attr = field.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>();
                                var name = attr != null ? attr.DisplayName : field.Name;
                                var text = $"{name}: {value}";
                                var memberNode = new TreeNode(text)
                                {
                                    Tag = value
                                };
                                memberNode.Nodes.Add(memberNode);
                                subNodes.Add(memberNode);
                            }

                            break;
                        }

                    case PropertyInfo property:
                    {
                        var type = property.PropertyType;
                        if (type == typeof(UArray<UObject>))
                        {
                            var value = (UArray<UObject>)property.GetValue(visitable);
                            if (value == null) continue;

                            var attr = property.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>();
                            var memberNode = new TreeNode(attr != null ? attr.DisplayName : property.Name);
                            foreach (var obj in value)
                            {
                                memberNode.Nodes.Add(new ObjectNode(obj));
                            }

                            subNodes.Add(memberNode);
                        }
                        else if (type.IsSubclassOf(typeof(UObject)))
                        {
                            var value = (UObject)property.GetValue(visitable);
                            if (value == null) continue;
                            var attr = property.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>();
                            var memberNode = new TreeNode(attr != null ? attr.DisplayName : property.Name)
                            {
                                Tag = value
                            };
                            memberNode.Nodes.Add(new ObjectNode(value));
                            subNodes.Add(memberNode);
                        }
                        //else if (type.GetInterface(nameof(IBinaryData)) != null)
                        //{
                        //    var value = (IBinaryData)property.GetValue(visitable);
                        //    if (value == null) continue;
                        //    var attr = property.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>();
                        //    var memberNode = new TreeNode(attr != null ? attr.DisplayName : property.Name)
                        //    {
                        //        Tag = value
                        //    };
                        //    subNodes.Add(memberNode);
                        //}
                        //else if (type.GetInterface(nameof(IUnrealSerializableClass)) != null)
                        //{
                        //    var value = (IUnrealSerializableClass)property.GetValue(visitable);
                        //    if (value == null) continue;
                        //    var attr = property.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>();
                        //    var memberNode = new TreeNode(attr != null ? attr.DisplayName : property.Name)
                        //    {
                        //        Tag = value
                        //    };
                        //    subNodes.Add(memberNode);
                        //}
                        break;
                    }
                }
            }

            return subNodes.Any() ? subNodes : null;
        }
    }
}