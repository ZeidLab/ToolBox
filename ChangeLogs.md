## Version 1.0.0

**Breaking Changes:**
- 🚨 Deprecated `Do<TIn>`, `DoIfSome<TIn>`, and `DoIfNone<TIn>` methods for `Maybe<TIn>`. These will be removed in the next major version.
  **Migration Path:** Use `Tap<TIn>`, `TapIfSome<TIn>`, and `TapIfNone<TIn>` instead for consistent behavior with `Result<TIn>`'s `Tap` method.

**Non-breaking Changes:**
- 📚 Updated documentation with comprehensive examples and explanations for improved clarity.

**New Features:**
- ✨ Added `Tap<TIn>`, `TapIfSome<TIn>`, and `TapIfNone<TIn>` extension methods to `Maybe<TIn>` for unified side-effect handling across monadic types.