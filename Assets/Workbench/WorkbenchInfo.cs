using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Workbench
{
    [CreateAssetMenu(fileName = "Workbench", menuName = "工作台")]
    public class WorkbenchInfo : ScriptableObject
    {
        public string Function;
        public string Date;
        public string Author;
        [Multiline(3)]public string Description;

        public List<DefaultAsset> DirectoryList = new List<DefaultAsset>();
    }
}
