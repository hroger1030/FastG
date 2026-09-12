# Geometry

A lightweight 2D/3D/nD geometry library written in C# for games and other applications that don't need
high-precision math. It favors speed and simplicity over precision — values are stored as `float`, so this
is **not** a good fit for serious scientific or CAD-grade math. It is, however, fast, easy to use, and easy
to extend with new shapes.

Every shape is an **immutable value type** (`readonly struct`) except `Polygon` and `VectorN`, which are
backed by collections. Passing a shape around, reading its `.Center`, or building one per frame costs no
heap allocation and produces no garbage. See [Value types and reference types](#value-types-and-reference-types).

The library is also tuned for the hot paths games actually hit every frame — see
[Performance](#performance).

Note that some objects are assumed to be grid-aligned (e.g. `Rectangle`, `Cube`, `AABB`). Making these
fully general (arbitrary rotation, etc.) is potential future work.

Test coverage is an ongoing effort. If you find a bug, please open an issue and it will be looked at as
soon as possible.

## Table of contents

- [Solution layout](#solution-layout)
- [File tree](#file-tree)
- [Requirements](#requirements)
- [Building and testing](#building-and-testing)
- [Objects](#objects)
- [Value types and reference types](#value-types-and-reference-types)
- [Performance](#performance)
- [Code examples](#code-examples)
  - [2D: points, vectors, and circles](#2d-points-vectors-and-circles)
  - [3D: bounding volumes](#3d-bounding-volumes)
  - [nD: arbitrary-dimension vectors](#nd-arbitrary-dimension-vectors)
- [License](#license)

## Solution layout

| Project | Description |
|---|---|
| [GeometryLib](GeometryLib) | The library itself. Namespace: `Geometry`. |
| [GeometryTests](GeometryTests) | NUnit test suite for the library, mirroring the `Objects` folder structure. |

## File tree

Only the files that matter for using or extending the library are listed below; build output
(`bin`/`obj`) and IDE folders are omitted.

```
Geometry/
├── Geometry.sln
├── LICENSE.txt
├── CLAUDE.md
├── GeometryLib/                    # Library project (namespace: Geometry)
│   ├── Geometry.csproj
│   ├── Interfaces/
│   │   ├── I1d.cs
│   │   ├── I2d.cs
│   │   └── I3d.cs
│   └── Objects/
│       ├── Constants.cs
│       ├── 2d/
│       │   ├── Point2.cs
│       │   ├── Vector2.cs
│       │   ├── Line2.cs
│       │   ├── Circle.cs
│       │   ├── Ellipse.cs
│       │   ├── Triangle2.cs
│       │   ├── Rectangle.cs
│       │   └── Polygon.cs
│       ├── 3d/
│       │   ├── Point3.cs
│       │   ├── Vector3.cs
│       │   ├── Ray.cs
│       │   ├── Plane3.cs
│       │   ├── Triangle3.cs
│       │   ├── Sphere.cs
│       │   ├── Cube.cs
│       │   ├── AABB.cs
│       │   └── Capsule.cs
│       └── Nd/
│           └── VectorN.cs
└── GeometryTests/                  # NUnit test project (namespace: GeometryTests)
    ├── GeometryTests.csproj
    └── Objects/                    # Mirrors GeometryLib/Objects/
        ├── 2d/
        ├── 3d/
        └── Nd/
```

## Requirements

- .NET 10 SDK
- Windows (target platform)

## Building and testing

```
dotnet build
dotnet test
```

## Objects

Every shape is an immutable value type (`readonly struct`) except where marked _(class)_; see
[Value types and reference types](#value-types-and-reference-types).

### Shared (`GeometryLib/Objects`)

- Constants — `FLOAT_ERROR_MARGIN`, the PI family (`PI`, `TWO_PI`, `HALF_PI`, `QUARTER_PI`), `DEG_TO_RAD`/`RAD_TO_DEG`,
  `SQRT_2`/`SQRT_3`, and their precomputed reciprocals (`INV_PI`, `INV_TWO_PI`, `INV_HALF_PI`, `INV_SQRT_2`,
  `INV_SQRT_3`) — see [Performance](#performance).

### 2D (`GeometryLib/Objects/2d`)

- Point2
- Vector2
- Line2
- Circle
- Ellipse
- Triangle2
- Rectangle
- Polygon _(class)_

### 3D (`GeometryLib/Objects/3d`)

- Point3
- Vector3
- Ray
- Plane3
- Triangle3
- Sphere
- Cube
- AABB
- Capsule

### Higher dimension (`GeometryLib/Objects/Nd`)

- VectorN _(class)_

### Interfaces (`GeometryLib/Interfaces`)

- I1d
- I2d
- I3d

## Value types and reference types

**Every shape is a `readonly struct`** — an immutable value type — with two exceptions:

- `Polygon` — backed by a `List<Point2>`.
- `VectorN` — backed by a `float[]`.

Those two stay `class`. Everything else (`Point2`, `Point3`, `Vector2`, `Vector3`, `Line2`, `Circle`,
`Ellipse`, `Triangle2`, `Triangle3`, `Rectangle`, `Ray`, `Plane3`, `Sphere`, `Cube`, `AABB`, `Capsule`)
lives on the stack (or inline in its container), is copied by value, is never `null`, and allocates no
heap memory.

What this means when you use them:

- **No `null`.** A `Point2` parameter can't be null, so there are no null-argument checks or
  `ArgumentNullException`s for the struct types. `default(Point2)` is the origin `(0, 0)`.
- **Immutable.** Properties are `{ get; init; }` — set them in a constructor or an object initializer,
  not afterwards. `rect.Left = 5;` will not compile. Produce a changed copy instead
  (`rect with { Left = 5 }`, or the `+` / `*` operators).
- **Value equality.** `==`, `!=`, `.Equals`, and `.GetHashCode` compare field values, so two separately
  constructed shapes with the same numbers are equal and hash the same. They work correctly as
  dictionary keys and in hash sets.
- **Cheap to pass and build.** Reading `rect.Center` or an item's bounding box every frame, or in a
  tight collision loop, does not allocate. This is the main reason for the conversion.

`Vector2`/`Vector3` used to have an in-place `Normalize()` that mutated the instance; it now returns a
unit-length copy (`v = v.Normalize();`). `Polygon` still has its mutating helpers.

## Performance

Beyond being allocation-free value types, the shapes and vectors in this library are specifically tuned
for tight, per-frame call sites — collision loops, per-vertex transforms, that kind of thing:

- **`readonly struct` everywhere it's feasible.** See [Value types and reference types](#value-types-and-reference-types).
  No heap allocation, no GC pressure, cheap to copy (most shapes are 8-24 bytes).
- **`[MethodImpl(MethodImplOptions.AggressiveInlining)]` on the hot members.** Arithmetic operators
  (`+`, `-`, `*`, `/`), the strongly-typed `Equals`/`==`/`!=`, and the core vector math (`Dot`, `Cross`,
  `Length`, `LengthSquared`, `DistanceTo`, `DistanceSquaredTo`, `Normalize`) all carry the hint, so the
  JIT doesn't have to guess — even across assembly boundaries, before tiered PGO has warmed up. It's
  deliberately *not* applied to anything with a loop (`VectorN`, `Polygon`) or multi-branch geometry
  tests (`Intersects`, `Contains`) — inlining those would bloat call sites without buying anything.
- **Precomputed constants instead of runtime division.** [`Constants`](GeometryLib/Objects/Constants.cs)
  provides `PI`/`TWO_PI`/`HALF_PI`/`QUARTER_PI`, `DEG_TO_RAD`/`RAD_TO_DEG`, `SQRT_2`/`SQRT_3`, and their
  reciprocals (`INV_PI`, `INV_TWO_PI`, `INV_HALF_PI`, `INV_SQRT_2`, `INV_SQRT_3`) as compile-time
  `const float`s. A multiply is cheaper than a divide on most hardware, so prefer `x * Constants.INV_PI`
  over `x / Constants.PI` on a hot path.

None of this changes behavior or API surface — it's all either compiler hints or drop-in constants, so
existing code keeps working unchanged.

## Code examples

### 2D: points, vectors, and circles

```csharp
using Geometry;

// Points and vectors
var start = new Point2(0f, 0f);
var end = new Point2(3f, 4f);
float distance = start.DistanceTo(end); // 5

var direction = new Vector2(end) - new Vector2(start);
direction = direction.Normalize();

Point2 moved = start + (direction * 2f); // move 2 units toward `end`

// Circles: overlap and containment checks
var a = new Circle(x: 0f, y: 0f, radius: 5f);
var b = new Circle(x: 6f, y: 0f, radius: 2f);

bool overlapping = a.Intersects(b);   // true, circles touch/overlap
bool inside = a.Contains(new Point2(1f, 1f)); // true

float area = a.Area;
float circumference = a.Circumference;
```

### 3D: bounding volumes

```csharp
using Geometry;

var sphere = new Sphere(new Point3(0f, 0f, 0f), radius: 5f);
bool hit = sphere.Contains(new Point3(1f, 2f, 3f));

var box = new AABB(
    min: new Point3(-1f, -1f, -1f),
    max: new Point3(1f, 1f, 1f));

var other = new AABB(
    min: new Point3(0.5f, 0.5f, 0.5f),
    max: new Point3(2f, 2f, 2f));

bool boxesOverlap = box.Intersects(other);
float volume = box.Volume;
```

### nD: arbitrary-dimension vectors

```csharp
using Geometry;

var v1 = new VectorN(4);
v1.Axis[0] = 1f;
v1.Axis[1] = 2f;
v1.Axis[2] = 3f;
v1.Axis[3] = 4f;

var v2 = new VectorN(4);
v2.Axis[0] = 4f;
v2.Axis[1] = 3f;
v2.Axis[2] = 2f;
v2.Axis[3] = 1f;

VectorN sum = v1 + v2;
VectorN scaled = v1 * 2f;
```

## License

This project is licensed under the [MIT License](LICENSE.txt).

In short: you can use, copy, modify, merge, publish, distribute, sublicense, and sell copies of this
software, in both personal and commercial projects, with no obligation to open-source your own code.
The only requirement is that the original copyright notice and license text are kept with any
substantial portion of the software you redistribute. The software is provided "as is," without
warranty of any kind — the authors are not liable for any claim or damages arising from its use.

See [LICENSE.txt](LICENSE.txt) for the full, legally-binding text.
