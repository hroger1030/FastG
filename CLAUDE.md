# CLAUDE.md

## Project overview

This repository contains a .NET solution for experimenting with a geometry library intended for use in games and other non-high precision applications.

## Stack

- C#
- Microsoft .NET 10
- Newtonsoft JSON parser
- NUnit tests 
- To be built for a Windows platform

## Development guidance

- Keep changes compatible with .NET 10.
- There should be only one namespace per project, and it should match the project name.
- Naming: PascalCase for types/members, camelCase for locals/parameters, SCREAMING_SNAKE_CASE for constants and static readonly constant-like fields (e.g. `Constants.PI`, `Circle.UNIT_CIRCLE`). No other snake_case, ever.
- For pairwise static methods over two different shape types (e.g. `Collisions2d.Intersects(AARectangle, Circle)`), order the parameters alphabetically by type name and implement the real logic exactly once at that signature. Never add a second overload just to accept the arguments in reverse order — if a caller has them backwards, have it swap the two arguments itself when it calls in. No redirect-only methods whose entire body is a call to a sibling overload with the arguments reversed. The `Ray`-vs-shape overloads (which return `(bool Hit, float Distance)` instead of a plain `bool`) are the one established exception: `Ray` is always listed first regardless of alphabetical order (e.g. `Intersects(Ray, AABB)`), for consistency across that whole family.
- No private functions allowed. methods should be public to allow unit tests to be written.
- set up functions to use dependency injection to allow for easy testing.
- Prefer small, focused updates to the geometry logic.
- If you add new features, update this file to reflect the new workflow.
- Don't upgrade any nuget package version without asking first. You can point out out of date packages to the user.
- Don't do write anything to git. You can read all you want, but no writes or commits.
- when refactoring existing code, do not remove comments. They can be updated if needed, but not removed.