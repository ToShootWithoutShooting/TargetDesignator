using UnityEngine;

public class GuideUICtl : MonoBehaviour
{
    public Transform m_target;                  //指向目标
    public RectTransform m_arrow;               //UI箭头
    Vector3 m_Screen_Center;

    private void Start()
    {
        m_Screen_Center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }
    
    void Update() {
        //将目标的世界坐标转换成屏幕坐标
        Vector3 target_ScreenPoint = Camera.main.WorldToScreenPoint(m_target.position);

        //计算target_ScreenPoint和屏幕中心点的绝对值角度
        float angle = Mathf.Atan(Mathf.Abs(target_ScreenPoint.y - Screen.height / 2)
            / Mathf.Abs(target_ScreenPoint.x - Screen.width / 2)) * 180 / Mathf.PI;

        //通过判断target_ScreenPoint相对屏幕中心点所在象限处理angle的差值
        if (target_ScreenPoint.x <= Screen.width / 2)
            angle = target_ScreenPoint.y >= Screen.height / 2 ? 90 - angle : 90 + angle;
        else
            angle = target_ScreenPoint.y >= Screen.height / 2 ? 270 + angle : 270 - angle;

        #region  计算摄像机和目标的叉乘

        //以屏幕中心所在的单位向量为起始向量from,并将from的y值设成和目标y值保持一致
        Vector3 from = Camera.main.transform.forward;
        from.y = m_target.forward.y;
        //将屏幕中心坐标转换成世界坐标
        Vector3 cameraPos = Camera.main.ScreenToWorldPoint(m_Screen_Center);
        cameraPos.y = m_target.position.y;
        
        //求出目标点与摄像机之间的向量
        Vector3 to = m_target.position - cameraPos;
        //求出两向量之间的叉乘
        Vector3 cross = Vector3.Cross(from, to);
        #endregion

        //根据叉乘求出带符号的角度
        //cross.y > 0:目标向量位于起始向量右侧
        //cross.y < 0:目标向量位于起始向量左侧
        if (cross.y > 0 && angle < 180)
            angle += 180;
        else if (cross.y < 0 && angle > 180)
            angle -= 180;
        Vector3 euler = m_arrow.eulerAngles;
        euler.z = angle;
        
        //设置箭头的角度
        m_arrow.eulerAngles = euler;
    }
}
