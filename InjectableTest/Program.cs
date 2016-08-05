using System;
using Ninject;
using Microsoft.Practices.Unity;

namespace InjectableTest
{
    internal interface IWeapon
	{
		void Hit(string target);
	}

    internal abstract class Weapon : IWeapon
	{
		public abstract void Hit(string target);
	}

    internal class Hadoken : Weapon
	{
		public override void Hit(string target)
		{
			Console.WriteLine("Hit {0} with HADOKEN", target);
		}
	}

    internal class Samurai
	{
        private readonly IWeapon _weapon;

		public Samurai(IWeapon weapon)
		{
			if (weapon == null)
				throw new ArgumentNullException(nameof(weapon));
			_weapon = weapon;
		}

		public void Attack(string target)
		{
			_weapon.Hit(target);
		}
	}

    internal interface IInjectable
	{
		void BindWeapon<TEntity>() where TEntity : Weapon;
		TWarrior GetWarrior<TWarrior>();
	}

    internal class Ninjector : IInjectable
	{
        readonly IKernel _kernel = new StandardKernel();
		public void BindWeapon<TEntity>() where TEntity:Weapon
		{
			var b = _kernel.Bind<IWeapon>();
			b.To<TEntity>();
		}

		public TWarrior GetWarrior<TWarrior>()
		{
			return _kernel.Get<TWarrior>();
		}
	}

    internal class Unityjector : IInjectable
	{
        private readonly IUnityContainer _container = new UnityContainer();
		public void BindWeapon<TEntity>() where TEntity : Weapon
		{
			_container.RegisterType<IWeapon, TEntity>();
		}

		public TWarrior GetWarrior<TWarrior>()
		{
			return _container.Resolve<TWarrior>();
		}
	}

    internal class Program
	{
        private static void Main()
		{
			//var n = new Unityjector();
			var n = new Ninjector();

			n.BindWeapon<Hadoken>();
			var warrior = n.GetWarrior<Samurai>();
			
			warrior.Attack("the opponent");
			Console.ReadLine();
		}
	}
}