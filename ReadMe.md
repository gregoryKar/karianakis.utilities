# Karianakis Utilities

Reusable Unity utilities focused on scheduling, IDs, pooling, and lightweight engine abstraction.

## Why This Package

`Karianakis.Utilities` helps you solve common gameplay and runtime problems with small, composable tools:

- Delayed and repeated execution (`Invo`, `InvoAdvanced`)
- Runtime IDs with grouping/overlap behavior (`MyId`, `MyChildId`)
- Generic object pooling with ID-aware bulk cleanup (`KarianakisPool<T>`, `PoolMaster`)
- Unity runtime connector that auto-bootstraps core systems
- Time helpers and timestamp utilities (`MyTime`, `TimeStamp`)

## Package Info

- Name: `com.karianakis.utilities`
- Current version: `1.0.6`
- Namespace: `Karianakis.Utilities`

## Modules

### 1) Invo Scheduler

A fast, heap-based invocation scheduler managed by `InvoManager`.

You can create:

- One-shot delayed calls
- Fixed repeat calls
- Infinite loops
- ID-scoped invocations (cancel/finish in groups)
- Pause/resume via ID

Entry points:

- `Invo.Simple(Action action, float delay)`
- `Invo.Repeat(Action action, float delay, int repeat)`
- `Invo.Infinite(Action action, float delay)`
- `InvoAdvanced.Simple(Action<InvoAdvancedBuilder> action, float delay)`
- `InvoAdvanced.Repeat(...)`
- `InvoAdvanced.Infinite(...)`

### 2) ID System

IDs are first-class runtime objects (`MyIdBase`) and can be attached to pooled items and invocations.

- `MyId`: unique runtime ID
- `MyChildId`: child ID linked to a parent ID
- Equality and overlap behavior are based on ID rules, not strict reference equality

Useful APIs:

- `SetPaused(bool paused)` on IDs to pause/resume related invocations
- `InvoManager.CancelAll(id)`
- `InvoManager.ForceFinishAll(id)`
- `InvoManager.Exists(id)`

### 3) Pooling

Generic object pool with extension points for instantiate/initialize/deactivate hooks.

Core classes:

- `KarianakisPool<T>`
- `PoolMaster`
- `KarianakisUnityPool<T> where T : Component`
- `UnityPoolMaster` (creates pool parent GameObjects automatically)

Features:

- Active/inactive tracking
- Typed retrieval (`Get<T>()`, `GetSigned<T>()`)
- ID tagging for pooled objects implementing `I_HaveIdExtended`
- Global cleanup (`RemoveAllActiveItems`) or cleanup by ID (`RemoveAllActiveItemsWithId`)

### 4) Unity Runtime Connector

`UnityUtilitiesConnector` initializes automatically before scene load:

- Creates a persistent runtime object (`DontDestroyOnLoad`)
- Registers `EngineConnector`
- Creates `InvoManager`
- Creates `IdLinkManager`

This means scheduling and ID-link functionality are ready at runtime without manual bootstrap in most setups.

### 5) Time Utilities

- `MyTime.GetNow`: current engine time via connector
- `MyTime.HasMommentPassed(...)`
- `TimeStamp`: lightweight comparable time marker struct

## Installation

### Option A: Unity Package Manager (Git URL)

In Unity:

1. Open `Window > Package Manager`
2. Click `+` and choose `Add package from git URL...`
3. Paste your repository URL, for example:

```text
https://github.com/<your-account>/<your-repo>.git?path=Assets/MyPackages/unity-utilities
```

### Option B: Embedded/Local Package

If this repository is already inside your Unity project, keep it under:

```text
Assets/MyPackages/unity-utilities
```

The package manifest (`package.json`) is already included.

## Quick Start

```csharp
using UnityEngine;
using Karianakis.Utilities;

public class UtilitiesQuickStart : MonoBehaviour
{
	MyId _groupId;

	void Start()
	{
		_groupId = new MyId();

		// Run once after 1 second
		Invo.Simple(() => Debug.Log("Hello once"), 1f)
			.SetId(_groupId)
			.SetFinishAction(() => Debug.Log("One-shot complete"));

		// Run 5 times every 0.5 seconds
		Invo.Repeat(() => Debug.Log("Tick"), 0.5f, 5)
			.SetId(_groupId)
			.SetCancelAction(() => Debug.Log("Canceled"));
	}

	[ContextMenu("Pause Group")]
	void PauseGroup() => _groupId.SetPaused(true);

	[ContextMenu("Resume Group")]
	void ResumeGroup() => _groupId.SetPaused(false);

	[ContextMenu("Cancel Group")]
	void CancelGroup() => InvoManager.CancelAll(_groupId);
}
```

## InvoAdvanced Example

Use `InvoAdvanced` when you want callback access to the running invocation.

```csharp
using UnityEngine;
using Karianakis.Utilities;

public class AdvancedInvoExample : MonoBehaviour
{
	void Start()
	{
		InvoAdvanced.Repeat(invo =>
		{
			Debug.Log($"Iteration: {invo.GetIterationIndex}");

			// Speed up after each iteration
			var nextDelay = Mathf.Max(0.05f, invo.GetDelay * 0.9f);
			invo.SetDelay(nextDelay);
		}, 0.25f, 10);
	}
}
```

## Pooling Example (Generic)

```csharp
using Karianakis.Utilities;

public class Bullet
{
	public bool Active;
}

public class BulletPoolExample
{
	readonly KarianakisPool<Bullet> _pool = new();

	public BulletPoolExample()
	{
		_pool.SetInstantiate(() => new Bullet());
		_pool.SetInitialize(b => b.Active = true);
		_pool.SetDeactivate(b => b.Active = false);
	}

	public Bullet Spawn() => _pool.Get();

	public void Despawn(Bullet bullet) => _pool.Remove(bullet);
}
```

## Pooling Example (Unity Components)

```csharp
using UnityEngine;
using Karianakis.Utilities;

public class Enemy : MonoBehaviour, I_HaveIdExtended
{
	MyIdBase _id;

	public MyIdBase GetId() => _id;

	public void SetId(MyIdBase id) => _id = id;
}

public class EnemyPoolInstaller : MonoBehaviour
{
	[SerializeField] Enemy _prefab;

	KarianakisUnityPool<Enemy> _pool;

	void Awake()
	{
		_pool = new KarianakisUnityPool<Enemy>();

		_pool.SetInstantiate(() => Instantiate(_prefab));
		_pool.SetInitialize(e => e.gameObject.SetActive(true));
		_pool.SetDeactivate(e => e.gameObject.SetActive(false));
	}

	public Enemy Spawn(string label, MyIdBase id)
		=> _pool.GetSigned(label, id);

	public void Despawn(Enemy enemy)
		=> _pool.Remove(enemy);
}
```

## ID Linking (Experimental)

`IdLinkManager` allows linking cleanup items to an ID.

- `IdLinkManager.Link(id, linkedItem)`
- `IdLinkManager.RemoveAllLinkedItems(id)`
- `IdLinkManager.ClearAll()`

Linked items must implement:

```csharp
public interface I_IdLinkedItem
{
	void RemoveMe();
}
```

## Notes

- The connector auto-initializes with `RuntimeInitializeOnLoadMethod(BeforeSceneLoad)`.
- Some folders are marked as experimental/in-progress in the source tree.
- Current roadmap ideas are tracked in `ToDoUtilities.md`.

