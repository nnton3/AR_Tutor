
using UnityEngine;

public class WordComposingCard : CardBase
{
    public bool FirstRankCard { get; private set; } = false;
    public bool SecondRankCard { get; private set; } = false;

    public override void Initialize(GameName _game, string _categoryKey, string cardKey, CardData data)
    {
        base.Initialize(_game, _categoryKey, cardKey, data);

        selectBtn.onClick.AddListener(() =>
        {
            if (MainMenuUIControl.Mode == MenuMode.Play)
                if (!WordComposingMenuControl.ClauseComplete)
                {
                    if (!FirstRankCard && !SecondRankCard)
                        Signals.LastWordSelected.Invoke();  

                    Signals.AddWordToClause.Invoke(GetComponent<CardBase>().Key);
                }
                Signals.PlayAudioClipEvent.Invoke(Clip);
        });
    }

    public void MarkCardAsRank(int _rank)
    {
        if (_rank == 1)
        {
            FirstRankCard = true;
            SecondRankCard = false;
            var texture = FindObjectOfType<SaveSystem>().LoadImage($"{FindObjectOfType<PatientDataManager>().GetPatientLogin()}clipName");
            if (texture != null)
            {
                var size = (texture.width > texture.height) ? texture.height : texture.width;
                image.sprite = Sprite.Create(texture, new Rect(0, 0, size, size), Vector2.zero);
            }
        }
        else if (_rank == 2)
        {
            FirstRankCard = false;
            SecondRankCard = true;
        }
    }

    protected override void SwitchVisible()
    {
        base.SwitchVisible();

        if (FirstRankCard)
        {
            if (editableElement.Visible)
                Signals.ReturnAllWordInClause.Invoke();
            else
                Signals.RemoveFirstWord.Invoke();
        }
        
        if (SecondRankCard)
        {
            if (editableElement.Visible)
                Signals.ReturnSecondRankCard.Invoke();
            else
                Signals.RemoveSecondRankWord.Invoke();
        }
    }
}
