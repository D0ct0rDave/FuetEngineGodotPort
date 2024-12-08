using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using Godot;

namespace FuetEngine
{
	[Tool]
	public class ResourceManager<T, M> where T : class where M : ResourceManager<T, M>, new()
	// public class ResourceManager<B, T> where B, T : class
	{
		// Singleton instance
		private static M _instance;

		// public static ResourceManager<T> Instance => _instance ??= new ResourceManager<T>();
		public static M Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new M();
				}
				return _instance;
			}
		}

		private readonly Dictionary<string, ResourceEntry> _resourceDatabase;

		protected ResourceManager()
		{
			_resourceDatabase = new Dictionary<string, ResourceEntry>();
		}

		public T Load(string filename)
		{
			if (_resourceDatabase.TryGetValue(filename, out var entry))
			{
				entry.ReferenceCount++;
				return entry.Resource;
			}

			T resource = LoadResource(filename);
			if (resource != null)
			{
				Register(resource, filename, true);
			}

			return resource;
		}

		public void Register(T resource, string resourceName, bool firstReference = false)
		{
			var entry = new ResourceEntry
			{
				Resource = resource,
				ReferenceCount = firstReference ? 1 : 0
			};

			_resourceDatabase[resourceName] = entry;
		}

		public void Reload()
		{
			var keys = new List<string>(_resourceDatabase.Keys);
			foreach (var filename in keys)
			{
				T newResource = LoadResource(filename);
				if (newResource != null)
				{
					var entry = _resourceDatabase[filename];
					entry.Resource = newResource;
				}
			}
		}

		public void Reset()
		{
			foreach (var entry in _resourceDatabase.Values)
			{
				InvalidateResource(entry.Resource);
			}
			_resourceDatabase.Clear();
		}

		public void ReleaseResource(T resource)
		{
			foreach (var kvp in _resourceDatabase)
			{
				if (kvp.Value.Resource == resource)
				{
					kvp.Value.ReferenceCount--;
					if (kvp.Value.ReferenceCount == 0)
					{
						InvalidateResource(resource);
						_resourceDatabase.Remove(kvp.Key);
					}
					break;
				}
			}
		}

		public bool Exists(string resourceName)
		{
			return _resourceDatabase.ContainsKey(resourceName);
		}

		public string GetResourceName(T resource)
		{
			foreach (var kvp in _resourceDatabase)
			{
				if (kvp.Value.Resource == resource)
					return kvp.Key;
			}

			return null;
		}

		protected virtual T LoadResource(string filename)
		{
			// This method should be overridden to provide the resource loading logic
			throw new NotImplementedException("LoadResource must be overridden in a derived class.");
		}

		protected virtual void InvalidateResource(T resource)
		{
			// This method should be overridden to provide resource invalidation logic
						// This method should be overridden to provide the resource loading logic
			throw new NotImplementedException("InvalidateResource must be overridden in a derived class.");
		}

		private class ResourceEntry
		{
			public T Resource { get; set; }
			public int ReferenceCount { get; set; }
		}
	}
}