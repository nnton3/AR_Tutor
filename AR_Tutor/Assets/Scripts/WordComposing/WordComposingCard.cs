using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
                Signals.AddWordToClause.Invoke(GetComponent<CardBase>().Key);
        });
    }

    public void MarkCardAsRank(int _rank)
    {
        if (_rank == 1)
        {
            FirstRankCard = true;
            SecondRankCard = false;
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
    }
}
