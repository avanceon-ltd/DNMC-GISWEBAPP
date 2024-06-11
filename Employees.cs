using System;

namespace WebAppForm
{
    [Serializable]
    public class Employees
    {

        public int EmpDbKey { get; set; }
        public string EmpName { get; set; }
        public string Address { get; set; }
    }
}