using System;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Procedure
{
    public class ProcedureLaunch : ProcedureBase
    {
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
            procedureOwner.SetData<VarInt16>("NextSceneId", 1);
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }
    }
}