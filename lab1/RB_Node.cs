using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class RB_Node
    {
        public RB_Node left; //левый потомок
        public RB_Node right;//правый потомок
        public RB_Node parent;//родитель

        public bool isRed;

        public int data;//значение

       // public int count; //количество элементов с таким значением

        //конструктор принимает значение узла
        public RB_Node() {}

        public RB_Node(int data)
        {
            this.data = data;
            this.left = null;
            this.right = null;
            this.parent = null;
         //   this.count = 1;
            this.isRed = false;
        }
        //конструктор, в котором можно указать родителя
        public RB_Node(int key, RB_Node parent) : this(key)
        {
            this.parent = parent;
        }
    }
}
