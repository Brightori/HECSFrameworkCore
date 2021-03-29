﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HECSFramework.Core
{
    public static class TypesMap
    {
        public static readonly int SizeOfComponents = 64;
        public static IMaskProvider MaskProvider;

        private static readonly Dictionary<int, ComponentMaskAndIndex> MapIndexes;
        private static readonly Dictionary<Type, int> TypeToComponentIndex;
        private static readonly Dictionary<Type, int> TypeToHash;
        private static readonly Dictionary<int, Type> componentHashToType;
        private static IComponentFactory componentFactory;

        static TypesMap()
        {
            MaskProvider = new MaskProvider();
            var typeProvider = new TypesProvider();
            MapIndexes = typeProvider.MapIndexes;
            TypeToComponentIndex = typeProvider.TypeToComponentIndex;
            SizeOfComponents = typeProvider.Count;
            componentFactory = typeProvider.ComponentFactory;
            TypeToHash = typeProvider.TypeToHash;
            componentHashToType = typeProvider.HashToType;
        }

        public static int GetHashOfComponentByType(Type type)
        {
            return TypeToHash[type];
        }

        public static bool GetComponentInfo(int hashTypeCode, out ComponentMaskAndIndex mask)
        {
            return MapIndexes.TryGetValue(hashTypeCode, out mask);
        }

        /// <summary>
        /// Static extention for getting by generic type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Owner"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IComponent GetHECSComponent<T>(this IEntity Owner)
        {
            var t = typeof(T);

            if (TypeToComponentIndex.TryGetValue(t, out var index))
            {
                return Owner.GetAllComponents[index];
            }
            else
                return default;
        }

        public static Type GetTypeByComponentHECSHash(int hash)
        {
            return componentHashToType[hash];
        }

        public static int GetIndexByType<T>() where T : IComponent
        {
            return TypeToComponentIndex[typeof(T)];
        }

        public static IComponent GetComponentFromFactory(int hashCodeType)
        {
            return componentFactory.GetComponentFromFactory(hashCodeType);
        }

        public static T GetComponentFromFactory<T>() where T : class, IComponent
        {
            return componentFactory.GetComponentFromFactory<T>();
        }
    }
}
