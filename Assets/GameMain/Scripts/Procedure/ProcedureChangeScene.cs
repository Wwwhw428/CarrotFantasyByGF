using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameMain.Scripts.DataTable;
using GameMain.Scripts.Definition.Constant;
using UnityGameFramework.Runtime;
using AssetUtility = GameMain.Scripts.Utility.AssetUtility;
using GameEntry = GameMain.Scripts.Base.GameEntry;

namespace GameMain.Scripts.Procedure
{
    public class ProcedureChangeScene : ProcedureBase
    {
        private bool _isChangeSceneComplete;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _isChangeSceneComplete = false;

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            var sceneId = procedureOwner.GetData<VarInt16>("NextSceneId");
            var dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            var drScene = dtScene.GetDataRow(sceneId);
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", sceneId.ToString());
                return;
            }

            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), Constant.AssetPriority.SceneAsset,
                this);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
            if (!_isChangeSceneComplete)
                return;
            
            ChangeState<ProcedureMain>(procedureOwner);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
            
            base.OnLeave(procedureOwner, isShutdown);
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            var ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
                return;

            Log.Info($"Load scene {ne.SceneAssetName} OK.");

            _isChangeSceneComplete = true;
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            var ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
                return;

            Log.Info($"Load scene '{ne.SceneAssetName}' fail, error message: {ne.ErrorMessage}");
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            var ne = (LoadSceneUpdateEventArgs)e;
            if (ne.UserData != this)
                return;

            Log.Info($"Load scene {ne.SceneAssetName} update, progress: {ne.Progress:P2}");
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            var ne = (LoadSceneDependencyAssetEventArgs)e;
            if (ne.UserData != this)
                return;

            Log.Info(
                $"Load scene {ne.SceneAssetName} dependency asset {ne.DependencyAssetName}, count {ne.LoadedCount}/{ne.TotalCount}");
        }
    }
}