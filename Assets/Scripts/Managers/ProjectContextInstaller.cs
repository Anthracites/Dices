using Zenject;
using Dices.UIConnection;

public class ProjectContextInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ScoreManager>().AsSingle().NonLazy();
        Container.Bind<SettingsManager>().AsSingle().NonLazy();
        Container.Bind<DonationManager>().AsSingle().NonLazy();
    }
}