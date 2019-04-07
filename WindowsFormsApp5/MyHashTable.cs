using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp5
{
    public class MyHashTable
    {

        public Hashtable values, formulas;
        public Hashtable Value
        {
            get
            {
                return values;
            }
        }
        public Hashtable Formulas
        {
            get
            {
                return formulas;
            }
        }
        static MyHashTable instance;
        private MyHashTable()
        {
            values = new Hashtable();
            formulas = new Hashtable();
        }
       /* public void clearData()
        {

        }*/
        public static MyHashTable GetInstance()
        {
            if (instance == null)
                instance = new MyHashTable();
            return instance;
        }
        public void AddFormula(string cell, string formula)
        {
            if (formulas.Contains(cell))
            {
                formulas[cell] = formula;
                return;
            }
            formulas.Add(cell, formula);
            formulas[cell] = formula;
        }
        public void AddValue(string cell, string value)
        {
            if (values.Contains(cell))
            {
                values[cell] = value;
                return;
            }
            values.Add(cell, value);
        }
        public void DeleteHash(string key)
        {
            formulas.Remove(key);
            values.Remove(key);
        }
        public void AddBoth(string cell, string formula, string value)
        {
            AddFormula(cell, formula);
            AddValue(cell, value);
        }
        public string getFormula(string cell)
        {
            if (formulas[cell] != null)
                return formulas[cell].ToString();
            return "";
        }
       
    }
}
