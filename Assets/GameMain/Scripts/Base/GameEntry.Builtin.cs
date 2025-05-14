using UnityGameFramework.Runtime;
using GFGameEntry = UnityGameFramework.Runtime.GameEntry;

namespace GameMain.Scripts.Base
{
    public partial class GameEntry
    {
        public static DataTableComponent DataTable { get; private set; }
        
        public static ProcedureComponent Procedure { get; private set; }
        
        public static SceneComponent Scene { get; private set; }
        
        public static ReferencePoolComponent ReferencePool { get; private set; }

        private static void InitBuiltinComponents()
        {
            DataTable = GFGameEntry.GetComponent<DataTableComponent>();
            Scene = GFGameEntry.GetComponent<SceneComponent>();
            Procedure = GFGameEntry.GetComponent<ProcedureComponent>();
            ReferencePool = GFGameEntry.GetComponent<ReferencePoolComponent>();
        }
    }
}