public class FeedbackState : StateWithView<FeedbackView>
{
    public override void EnterState()
    {
        base.EnterState();
        
        view.SubscribeButton(FeedbackView.FV_ButtonType.GOOD_FEEDBACK,GoodFeedback);
        view.SubscribeButton(FeedbackView.FV_ButtonType.MEH_FEEDBACK,MediumFeedback);
        view.SubscribeButton(FeedbackView.FV_ButtonType.BAD_FEEDBACK,BadFeedback);
        view.SubscribeButton(FeedbackView.FV_ButtonType.BACK, fsm.ChangeState<MainMenuState>);
    }
    private void GoodFeedback()
    {
        WriteFeedbackController.WriteFeedback(3);
        fsm.ChangeState<MainMenuState>();
    }

    private void MediumFeedback()
    {
        WriteFeedbackController.WriteFeedback(2);
        fsm.ChangeState<MainMenuState>();
    }
    
    private void BadFeedback()
    {
        WriteFeedbackController.WriteFeedback(1);
        fsm.ChangeState<MainMenuState>();
    }
}
