using Zenject;
using Dices.UIConnection;
using Dices.GamePlay;
using UnityEngine;

public class ProjectContextInstaller : MonoInstaller
{
    public UnityEngine.Object DicePref;
    public override void InstallBindings()
    {
        BindManagers();
        BindFactories();
    }

    void BindManagers()
    {
        Container.Bind<ScoreManager>().AsSingle().NonLazy();
        Container.Bind<SettingsManager>().AsSingle().NonLazy();
        Container.Bind<DonationManager>().AsSingle().NonLazy();
    }

    void BindFactories()
    {
        Container.BindFactory<Object, DiceRotation, DiceRotation.Factory>().FromFactory<PrefabFactory<DiceRotation>>();

        //        Container.BindFactory<string, DiceRotation, DiceRotation.Factory>().FromFactory<PrefabResourceFactory<DiceRotation>>(); // префаб по пути 

    }
}