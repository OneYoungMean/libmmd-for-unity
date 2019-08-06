using LibMMD.Unity3D;
using UnityEngine;

namespace LibMmdDemo
{
	public class BasicDemoController : MonoBehaviour
	{
		public string ModelPath;

		public string MotionPath;
		
		public string CameraPath;
        //OYM：好吧...看来是非要把这个插件肝穿不可
        protected void Start ()
		{
			if (string.IsNullOrEmpty(ModelPath)
                //|| string.IsNullOrEmpty(MotionPath) || string.IsNullOrEmpty(CameraPath)这里先屏蔽下
                )
			{
				Debug.LogError("please fill your model, motion and camera file path");
			}
            var mmdObj = MmdGameObject.CreateGameObject("MmdGameObject");//OYM：创造一个mmd播放,这里值得我参考一下
            var mmdGameObject = mmdObj.GetComponent<MmdGameObject>();//OYM：获取上面的组件

            mmdGameObject.LoadModel(ModelPath);//OYM：加载模型
            mmdGameObject.LoadMotion(MotionPath);//OYM：加载动作

            //You can set model render options
            //OYM：设置模型渲染模式(这里先不去动他)
            mmdGameObject.UpdateConfig(new MmdUnityConfig
			{
				EnableDrawSelfShadow = MmdConfigSwitch.ForceFalse,
				EnableCastShadow = MmdConfigSwitch.ForceFalse
			});

            mmdGameObject.Playing = true;//OYM：设置播放

            //OYM：下面三个都是相机导入和播放的,这里也别去碰他,现在还不需要用到
            //var mmdCamera = MmdCameraObject.CreateGameObject("MmdCameraObject").GetComponent<MmdCameraObject>();
            //mmdCamera.LoadCameraMotion(CameraPath);
            //mmdCamera.Playing = true;

        }

		
	}
}
