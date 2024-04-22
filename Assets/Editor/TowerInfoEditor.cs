using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TowerInfo))]
public class TowerInfoEditor : Editor
{
    private bool upgradeInfoFoldout = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TowerInfo towerInfo = (TowerInfo)target;

        EditorGUILayout.Space();

        upgradeInfoFoldout = EditorGUILayout.Foldout(upgradeInfoFoldout, "Upgrade Info");
        if (upgradeInfoFoldout)
        {
            EditorGUI.indentLevel++;

            DisplayUpgradePath(towerInfo.path1, "Path 1 Upgrades");
            DisplayUpgradePath(towerInfo.path2, "Path 2 Upgrades");
            DisplayUpgradePath(towerInfo.path3, "Path 3 Upgrades");

            EditorGUI.indentLevel--;
        }
    }

    private void DisplayUpgradePath(Upgrade[] upgrades, string label)
    {
        EditorGUILayout.LabelField(label);
        EditorGUI.indentLevel++;
        foreach (var upgrade in upgrades)
        {
            upgrade.upgradeName = EditorGUILayout.TextField("Upgrade Name", upgrade.upgradeName);
            upgrade.price = EditorGUILayout.IntField("Price", upgrade.price);
            EditorGUILayout.Space();
        }
        EditorGUI.indentLevel--;
    }
}
