namespace CommonGround.LifeCycle {
    using ICities;
    using CommonGround.Tool;

    public class ThreadingExtension : ThreadingExtensionBase{
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta) {
            var tool = ToolsModifierControl.toolController?.CurrentTool;
            bool flag = tool == null || tool is CommonGroundTool ||
                tool.GetType() == typeof(DefaultTool) || tool is NetTool || tool is BuildingTool;
            if (flag && CommonGroundTool.ActivationShortcut.IsKeyUp()) {
                CommonGroundTool.Instance.ToggleTool();
            }
        }
    }
}
