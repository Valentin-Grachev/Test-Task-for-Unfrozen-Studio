using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// —труктура, хран€ща€ название слота и вложени€
[System.Serializable]
public struct AttachementPath
{
	[SpineSlot] public string slotName;
	[SpineAttachment(slotField: "slotName")] public string attachmentName;
}


// —истема наложени€ заданных частей одного скина поверх другого
public class OverlaySkin : MonoBehaviour
{
	[SpineSkin][SerializeField] private string _baseSkinName;
	[SpineSkin][SerializeField] private string _overlaySkinName;
	[SerializeField] private List<AttachementPath> _overlayAttachements;
	public List<int> overlayIndices;

	private List<int> _notOverlayIndices;
	private Skeleton _skeleton;

	private const float overlayBrigthess = 0.8f;
	private const float maxOverlayIntensity = 0.7f;
	private const float speedMakingInvisible = 2f;



    public void Initialize()
	{
		Character character = GetComponent<Character>();
		character.onHealthChanged += Character_OnHealthChanged;
        character.onDeath += Character_OnDeath;
		_skeleton = GetComponent<SkeletonAnimation>().skeleton;
		_notOverlayIndices = new List<int>();
	}

    private void Character_OnDeath()
    {
		StartCoroutine(MakeInvisible());
    }

    private void Character_OnHealthChanged(int health, int maxHealth)
    {
		float bloodIntensity = maxOverlayIntensity - (float)health / (float)maxHealth*maxOverlayIntensity;
		UpdateOverlaySkin(overlayBrigthess, bloodIntensity);
    }

    private void UpdateOverlaySkin(float colorBrightness, float overlaySkinAlpha)
    {
		// «аполн€ем список дл€ удалени€ вложений - туда не вход€т индексы из списка на добавление вложений
		_notOverlayIndices.Clear();
		for (int i = 0; i < _overlayAttachements.Count; i++) _notOverlayIndices.Add(i);
		foreach (var item in overlayIndices) _notOverlayIndices.Remove(item);

		// ѕолное копирование базового и наложенного скина
		var baseSkin = _skeleton.Data.FindSkin(_baseSkinName);
		var overlaySkin = _skeleton.Data.FindSkin(_overlaySkinName);
		Skin newSkin = new Skin("NewSkin");
		baseSkin.CopyTo(newSkin, true, true);
		overlaySkin.CopyTo(newSkin, true, true);
		
		// ”даление вложений, вз€тых по индексу из списка неналожени€ вложений
		foreach (var index in _notOverlayIndices) RemoveAttachement(_overlayAttachements[index], newSkin);

		// ”становка нового комбинированного скина
		_skeleton.SetSkin(newSkin);
		_skeleton.SetSlotsToSetupPose();

		// ”становка прозрачности дл€ наложенного скина
		foreach (var item in _overlayAttachements)
		{
			Slot slot = _skeleton.FindSlot(item.slotName);
			slot.SetColor(new Color(colorBrightness, colorBrightness, colorBrightness, overlaySkinAlpha));
		}

		_skeleton.Update(0);
		

	}


	private void RemoveAttachement(AttachementPath path, Skin skin)
    {
		int index = _skeleton.data.FindSlotIndex(path.slotName);
		skin.RemoveAttachment(index, path.attachmentName);
    }

	private IEnumerator MakeInvisible()
    {
        while (_skeleton.A > 0)
        {
			yield return new WaitForEndOfFrame();
			if ((_skeleton.A - Time.deltaTime * speedMakingInvisible) <= 0) _skeleton.A = 0;
			else _skeleton.A -= Time.deltaTime * speedMakingInvisible;

		}
		
    }



}
