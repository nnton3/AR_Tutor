using System.Collections.Generic;
using UnityEngine;

public interface IGameMenu
{
    /// <summary>
    /// Инициализация всех переменных и задание начальных параметров
    /// </summary>
    void Initialize();
    /// <summary>
    /// Добавление карточки в категорию из хранилища (CardStorage)
    /// </summary>
    /// <param name="_game">Тип игры</param>
    /// <param name="_categoryIndex">Индекс категории в списке</param>
    /// <param name="_key">Ключ карточки, по которому происходит поиск и идентификация карточки</param>
    void AddNewCard(GameName _game, int _categoryIndex, string _key);
    /// <summary>
    /// Удаление карточки из категории
    /// </summary>
    /// <param name="_categoryIndex">Индекс категории в списке</param>
    /// <param name="_key">Ключ карточки, по которому происходит поиск и идентификация карточки</param>
    void DeleteCard(GameName _game, int _categoryIndex, string _key);
    /// <summary>
    /// Обновление картинки карточки
    /// </summary>
    /// <param name="_cardKey">Ключ карточки, по которому происходит поиск и идентификация карточки</param>
    /// <param name="_cardImg">Новое изображение, которое необходимо поставить вместо текущего</param>
    void UpdateCardImg(string _cardKey, Sprite _cardImg);
}

public interface IManageCards
{
    void Initialize(List<GameObject> _cards);
    void AddCard(GameObject _card);
    void RemoveCard(GameObject _card);
}
