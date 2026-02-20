# Unity C# Style Guide

This project follows standard Unity C# conventions, with the rules below treated as team defaults.

## 1) Do not use public fields

- Never expose mutable state with `public` variables.
- Use `[SerializeField] private` for Inspector-visible fields.
- Expose data to other scripts through properties or explicit methods.

```csharp
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
  [SerializeField] private float moveSpeed = 5f;
  [SerializeField] private int maxHealth = 100;

  public float MoveSpeed { get; private set; }; // Read-only access

  public void SetMoveSpeed(float value)
  {
    moveSpeed = Mathf.Max(0f, value);
  }
}
```

## 2) Prefer guard clauses (early return)

- Exit invalid or irrelevant states first.
- Keep the main execution path flat and easy to scan.

```csharp
void Update()
{
  if (!controller.isGrounded) return;
  if (!Input.GetButtonDown("Jump")) return;

  Jump();
}
```

Avoid deep nesting when a guard clause is clearer.

## 3) Use 2 spaces for indentation

- Indent with spaces, not tabs.
- Standard indentation depth is 2 spaces.

```csharp
if (canMove)
{
  MovePlayer();
}
```

## 4) Avoid the `var` keyword

## Additional Style Callouts To Consider

1. Name private serialized fields in `camelCase` and keep them near the top of the class.
2. Use `PascalCase` for classes, methods, properties, and public APIs.
3. Keep `Update()` focused: gather input, then delegate to small methods (`HandleMovement()`, `HandleJump()`, etc.).
4. Cache component references in `Awake()`/`Start()` instead of repeatedly calling `GetComponent()` in `Update()`.
5. Use `const`/`readonly` for values that should not change at runtime.
6. Always validate serialized references before use with guard clauses.
7. One class per file, and file name should match class name.
8. Keep methods short and single-purpose when possible.
