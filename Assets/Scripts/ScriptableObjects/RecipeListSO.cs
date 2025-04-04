using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeListSO", menuName = "Scriptable Objects/RecipeListSO")]
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> recipeSOList; // 레시피 리스트
}
