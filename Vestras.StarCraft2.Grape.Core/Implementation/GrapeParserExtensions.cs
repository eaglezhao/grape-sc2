using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	internal static class GrapeParserExtensions {
		public static GrapeModifier.GrapeModifierType Merge(this GrapeList<GrapeModifier> modifiers, GrapeModifier.GrapeModifierType defaultAccess) {
			Debug.Assert((defaultAccess != GrapeModifier.GrapeModifierType.Default) && ((defaultAccess&~GrapeModifier.GrapeModifierType.Access) == 0));
			GrapeModifier.GrapeModifierType result = default(GrapeModifier.GrapeModifierType);
			foreach (GrapeModifier modifier in modifiers.Enumerate()) {
				result |= modifier.Type;
			}
			if ((result&GrapeModifier.GrapeModifierType.Access) == GrapeModifier.GrapeModifierType.Default) {
				result |= defaultAccess;
			}
			return result;
		}

		public static IEnumerable<T> Enumerate<T>(this GrapeList<T> sequence) where T: GrapeEntity {
			return sequence.Enumerate(null);
		}

		public static IEnumerable<T> Enumerate<T>(this GrapeList<T> sequence, params T[] prepend) where T: GrapeEntity {
			if (prepend != null) {
				foreach (T item in prepend) {
					yield return item;
				}
			}
			while (sequence != null) {
				if (sequence.Item != null) {
					yield return sequence.Item;
				}
				sequence = sequence.Next;
			}
		}

		public static List<T> ToList<T>(this GrapeList<T> sequence, GrapeEntity newParent) where T: GrapeEntity {
			return sequence.ToList(newParent, null);
		}

		public static List<T> ToList<T>(this GrapeList<T> sequence, GrapeEntity newParent, params T[] prepend) where T: GrapeEntity {
			List<T> result = new List<T>();
			foreach (T item in sequence.Enumerate(prepend)) {
				item.Parent = newParent;
				result.Add(item);
			}
			return result;
		}

		public static string GetFullName<T>(this GrapeList<T> qualifiedId) where T: GrapeIdentifier {
			return string.Join(".", qualifiedId.Enumerate().Select(i => i.Name));
		}

		public static string GetPackageName<T>(this GrapeList<T> qualifiedId) where T: GrapeIdentifier {
			T simpleName = qualifiedId.Enumerate().Last();
			return string.Join(".", qualifiedId.Enumerate().TakeWhile(i => i != simpleName).Select(i => i.Name));
		}

		public static string GetSimpleName<T>(this GrapeList<T> qualifiedId) where T: GrapeIdentifier {
			return qualifiedId.Enumerate().Last().Name;
		}
	}
}
