//
// @brief: 管理基类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/10
// 
// 
//

public class BaseManager {
    protected GameFacade facade;
    public BaseManager(GameFacade facade)
    {
        this.facade = facade;
    }
    public virtual void OnInit() { }
    public virtual void Update() { }
    public virtual void OnDestroy() { }
}
