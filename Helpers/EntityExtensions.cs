﻿using System.Runtime.CompilerServices;
using Systems;

namespace HECSFramework.Core
{
    internal static class EntityExtensions
    {
        /// <summary>
        /// Это самый быстрый способ получить или добавить и получить компонент, в переборах и циклах лучше использовать его чем GetOrAdd
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="mask"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetOrAddComponent<T>(this IEntity entity, HECSMask mask) where T: class, IComponent
        {
            if (entity.TryGetHecsComponent(mask, out T component))
                return component;
            else
            {
                var comp = entity.World.GetSingleSystem<PoolingSystem>().GetComponentFromPool<T>(ref mask);
                entity.AddHecsComponent(comp);
                return comp;
            }
        }

        /// <summary>
        /// Удобная проверка, проверяет сразу все: существует ли энтити и жива ли она
        /// </summary>
        public static bool IsAlive(this IEntity entity)
            => EntityManager.IsAlive && entity != null && entity.IsAlive;
    }
}