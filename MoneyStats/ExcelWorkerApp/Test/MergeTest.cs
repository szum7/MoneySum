using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelWorkerApp.Test
{
    public class MergeTest
    {
        public List<MyObj> MergeTestMethod()
        {
            var tr1 = new List<MyObj>()
            {
                new MyObj() { id = 1, title = "a" },
                new MyObj() { id = 1, title = "b" },
                new MyObj() { id = 2, title = "a" },
                new MyObj() { id = 2, title = "b" },
                new MyObj() { id = 2, title = "c" },
                new MyObj() { id = 3, title = "a" },
                new MyObj() { id = 3, title = "b" },
                new MyObj() { id = 3, title = "c" },
                new MyObj() { id = 3, title = "d" },
            };
            var tr2 = new List<MyObj>()
            {
                new MyObj() { id = 2, title = "b" },
                new MyObj() { id = 2, title = "c" },
                new MyObj() { id = 2, title = "d" },
                new MyObj() { id = 3, title = "a" },
                new MyObj() { id = 3, title = "b" },
            };

            List<MyObj> merged = new List<MyObj>();
            int i = 0, j = 0;
            while (i < tr1.Count && j < tr2.Count)
            {
                if (tr1[i].id < tr2[j].id)
                {
                    merged.Add(tr1[i]);
                    i++;
                }
                else if (tr1[i].id > tr2[j].id)
                {
                    merged.Add(tr1[2]);
                    j++;
                }
                else
                {
                    while (i < tr1.Count && j < tr2.Count && tr1[i].id == tr2[j].id)
                    {
                        if (!merged.Any(x => x.ToString() == tr1[i].ToString()))
                        {
                            merged.Add(tr1[i]);
                        }
                        if (tr1[i].ToString() != tr2[j].ToString())
                        {
                            if (!merged.Any(x => x.ToString() == tr2[j].ToString()))
                            {
                                merged.Add(tr2[j]);
                            }
                            j++;
                        }
                        i++;
                    }
                    i++;
                    j++;
                }
            }
            while (i < tr1.Count)
            {
                merged.Add(tr1[i]);
                i++;
            }
            while (j < tr2.Count)
            {
                merged.Add(tr2[j]);
                j++;
            }
            return merged;
        }
    }
}
