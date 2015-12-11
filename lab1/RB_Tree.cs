using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Msagl;

namespace lab1
{
    public class RB_Tree
    {

        public Form1 form;
        public RB_Node root;

        public RB_Tree(int data, Form1 form) // конструктор для создания дерева начиная с корня
        {
            this.root = new RB_Node(data);
            this.form = form;
        }

        public void RB_Insert(int key) // вставка в дерево
        {
            RB_Node z = new RB_Node(key);
            if (Search(key) == false) // если такого элемента нет
            {
                RB_Node y = null;
                RB_Node x = root;
                while (x != null)
                {
                    y = x;
                    if (z.data < x.data) //y заменил на x
                        x = x.left;
                    else
                        x = x.right;
                }
                z.parent = y;
                if (y == null)
                    root = z;
                else
                {
                    if (z.data < y.data)
                        y.left = z;
                    else
                        y.right = z;
                }
                ///////////////////////////from book
                // мы это и так не инициализируем, поу-молчанию тут null
                //z.left = null;
                //z.right = null;
                z.isRed = true;
                ///////////////////////////
                RB_Insert_Fixup(z);
            }
           // else
                //++Tree_Search(root, key).count;
        }

        public void RB_Insert_Fixup(RB_Node z) // переставляет элементы согласно правилам RB_Tree
        {
            while (z != root && z.parent.isRed) // z!= root для того что бы не делать родитель корня 
            {
                if (z.parent.parent != null && z.parent == z.parent.parent.left)
                {
                    RB_Node y = z.parent.parent.right;
                    if (y != null && y.isRed) // f! = null когда уходит влево ветка (не создаю нулевые листья) 
                    {
                        z.parent.isRed = false;
                        y.isRed = false;
                        z.parent.parent.isRed = true;
                        z = z.parent.parent;

                    }
                    else
                    {
                        if (z == z.parent.right)
                        {
                            z = z.parent;
                            Left_Rotate(z);
                        }
                        z.parent.isRed = false;
                        z.parent.parent.isRed = true;
                        Right_Rotate(z.parent.parent);
                        ///////////////////////////
                        root.parent = null;
                        ///////////////////////////
                    }
                }
                else
                {
                    RB_Node y = z.parent.parent.left;
                    if (y != null && y.isRed) // f! = null когда уходит вправо ветка (не создаю нулевые листья) 
                    {
                        z.parent.isRed = false;
                        y.isRed = false;
                        z.parent.parent.isRed = true;
                        z = z.parent.parent;
                    }
                    else
                    {
                        if (z == z.parent.left)
                        {
                            z = z.parent;
                            Right_Rotate(z);
                        }
                        z.parent.isRed = false;
                        z.parent.parent.isRed = true;
                        Left_Rotate(z.parent.parent);
                        ///////////////////////////
                        root.parent = null;
                        ///////////////////////////
                    }
                }
            }
            root.isRed = false;
        }

        public void Left_Rotate(RB_Node x) // левый поворот
        {
            RB_Node f = x.right;
            x.right = f.left;
            if (f.left != null)
                f.left.parent = x;
            ///////////////////////////
            if (x.parent != null)
            ///////////////////////////
             f.parent = x.parent;
            if (x.parent == null)
                root = f;
            else
            {
                if (x == x.parent.left)
                    x.parent.left = f;
                else
                    x.parent.right = f;
            }
            f.left = x;
            ///////////////////////////
            if (x.parent == null)
                root = f;
            ///////////////////////////
            x.parent = f;
        }

        public void Right_Rotate(RB_Node x) // правый поворот
        {
            RB_Node f = x.left;
            x.left = f.right;
            if (f.right != null)
                f.right.parent = x;
            ///////////////////////////
            if (x.parent!=null)
            ///////////////////////////
                f.parent = x.parent;

            if (x.parent == null)
                root = f;
            else
            {
                if (x == x.parent.right)
                    x.parent.right = f;
                else
                    x.parent.left = f;
            }
            f.right = x;
            ///////////////////////////
            if (x.parent == null)
                root = f;
            ///////////////////////////
            x.parent = f;
        }

        RB_Node Tree_Search(RB_Node x, int k) // возвращает искомый элемент из узла x (как правило с корня) по ключу k
        {
            if (x == null || k == x.data)
            return x;
            if (k < x.data)
                return Tree_Search(x.left, k);
            else
                return Tree_Search(x.right, k);
        }

        /*RB_Node Tree_Successor(ref RB_Node x) // возвращает узел, следующий за узлом x в бинарном дереве поиска(если такой существует)
        { // и null, если x обладает наибольшем ключом в бинарном дереве
            RB_Node y = null;
            if (x.right != null)
                return Tree_Minimum(ref x.right);
            y = x.parent;
            while (y != null && x == y.right)
            {
                x = y;
                y = y.parent;
            }
            return y;
        }*/
        public bool Search(int k) // поиск элемента по ключу k
            {
                if (Tree_Search(root, k) != null)
                    return true;
                else
                    return false;
            }

        Microsoft.Msagl.GraphViewerGdi.GViewer viewer;
        Microsoft.Msagl.Drawing.Graph graph;

        public void printTree() //вывод всего дерева
        {
            viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            graph = new Microsoft.Msagl.Drawing.Graph("graph");

           /* graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
            graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
            graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
            Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");
            c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
            c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
            * */
            print(root);

            viewer.Graph = graph;

            form.tabControl1.TabPages[1].SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Top;
            form.tabControl1.TabPages[1].Controls.Add(viewer);
            form.tabControl1.TabPages[1].ResumeLayout();

        }

        void print(RB_Node node) //вывод поддерева
        {
            if (node == null)
                return;
            print(node.right);

            string line = node.data.ToString();
            line += (node.isRed) ? " red" : " black";
            if (node.parent != null)
            {
                graph.AddEdge(node.parent.data.ToString(),node.data.ToString());
                if (node.isRed)
                graph.FindNode(node.data.ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                line += (node == node.parent.left) ? " – and – left" : " – and – right";
            }
                
            if (node.parent != null)
                line += " son: " + node.parent.data.ToString();
            //form.richTextBox1.Text += line;
            print(node.left);
        }

    }
}
