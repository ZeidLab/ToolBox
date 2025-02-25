# ToolBox Change logs

## [v1.1.2502.2422](https://github.com/ZeidLab/ToolBox/releases/tag/v1.1.2502.2422)

### Breaking Changes ⚠️

#### Deprecated Methods

* `Tap()` method for `Maybe<TIn>` has been deprecated in favor of `TapIfSome()`/`TapIfNone()`.
* `Map()` method for `Maybe<TIn>` has been deprecated in favor of `Match()`, consistent with the `Match` method for `Result<TIn>`.

### New Features ✨

This release introduces extensive support for async workflows and method chaining for both `Maybe<TIn>` and `Result<TIn>`.

* **Bind/BindAsync Methods for `Maybe<TIn>`:**
	* Support for mixed synchronous/asynchronous handler workflows.
	* Fluent composition with both void and return-type handlers.
	* Support for method chaining.

* **Match/MatchAsync Methods for `Maybe<TIn>`:**
	* State-aware pattern matching capabilities.
	* Async-compatible result projections.

* **TapIfSome/TapIfNone Methods for `Maybe<TIn>`:**
	* Added async side effects support with `TapIfSomeAsync`/`TapIfNoneAsync`.

* **Ensure/EnsureAsync Methods for `Result<TIn>`:**
	* Support for `Try<TIn>` and `TryAsync<TIn>`.
	* Chainable `Try<TIn>` and `TryAsync<TIn>` with `Result<TIn>`.

* **Bind/BindAsync Methods for `Result<TIn>`:**
	* Support for mixed synchronous/asynchronous handler workflows.
	* Chainable `Try<TIn>` and `TryAsync<TIn>` with `Result<TIn>` and `Task<Result<TIn>>`.
	* Fluent composition with both void and return-type handlers.

* **Match/MatchAsync Methods for `Result<TIn>`:**
	* Chainable `Try<TIn>` and `TryAsync<TIn>` with `Result<TIn>` and `Task<Result<TIn>>`.
	* State-aware pattern matching capabilities.
	* Async-compatible result projections.
* Added support for `Bind`/`BindAsync` methods for `Try<TIn>` to enable:
	* Seamless integration with `Result<TIn>` and `Maybe<TIn>` types
	* Enhanced error handling and propagation

### Improvements 📈

#### Documentation Updates

* Added complete usage examples for core API surface
* Expanded conceptual explanations for:
	* Error handling strategies
	* Async/Await best practices
	* Composition patterns with `Bind` and `Match`
* Included troubleshooting guide for common scenarios
* Improved API reference documentation with XML doc enhancements

### Fixes 🛠️

* Resolved a bug in `MatchAsync` methods that caused incorrect result projections under specific conditions.
* Corrected documentation inconsistencies and improved XML comments for better IntelliSense support.


## [v1.1.2502.2013](https://github.com/ZeidLab/ToolBox/releases/tag/v1.1.2502.2013)

### Breaking Changes ⚠️

* **No breaking changes** in this release.

### New Features ✨

#### Enhanced Method Overloads

* Added new overloads for `Bind`/`BindAsync` methods to support:
  * Mixed synchronous/asynchronous handler workflows
  * Simplified error propagation scenarios
  * Fluent composition with both void and return-type handlers
* Extended `Match`/`MatchAsync` methods with:
  * State-aware pattern matching capabilities
  * Async-compatible result projections
  * Optional fallback handlers for unhandled cases

#### Added `DebuggerDisplay` attribute

The attribute added to `maybe<TIn>`, `Result<TIn>`, and `ResultError` types to improve debuging experience.

### Improvements 📈

#### Documentation Updates

* Added complete usage examples for core API surface
* Expanded conceptual explanations for:
  * Error handling strategies
  * Async/Await best practices
  * Composition patterns with `Bind` and `Match`
* Included troubleshooting guide for common scenarios
* Improved API reference documentation with XML doc enhancements

## [v1.1.2502.1618](https://github.com/ZeidLab/ToolBox/releases/tag/v1.1.2502.1618)

**Breaking Changes:**

* 🚨 Deprecated `Do<TIn>`, `DoIfSome<TIn>`, and `DoIfNone<TIn>` methods for `Maybe<TIn>`. These will be removed in the next major version.
  **Migration Path:** Use `Tap<TIn>`, `TapIfSome<TIn>`, and `TapIfNone<TIn>` instead for consistent behavior with `Result<TIn>`'s `Tap` method.

**New Features:**

* ✨ Added `Tap<TIn>`, `TapIfSome<TIn>`, and `TapIfNone<TIn>` extension methods to `Maybe<TIn>` for unified side effect handling across monadic types.

**Improvements:**

* 📚 Updated documentation with comprehensive examples and explanations for improved clarity.
