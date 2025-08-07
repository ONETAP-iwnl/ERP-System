'use client'

import { useRouter, useParams } from 'next/navigation';
import { useEffect, useState } from 'react';
import styles from "@/styles/recipts-page/hero/hero.module.css"
import { getResourceById, updateResource, archiveResource, deleteResource } from '@/services/api/resourceClient';


export default function Hero() {
    const router = useRouter();
    const params = useParams();
    const id = Number(params.id);

    const [inputName, setInputName] = useState('');
    const [isActive, setIsActive] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        async function fetchResource() {
          try {
            const resource = await getResourceById(id);
            setInputName(resource.name);
            setIsActive(resource.isActive);
          } catch (e) {
            setError('Ошибка при загрузке ресурса');
          }
        }
        if (id) {
          fetchResource();
        }
      }, [id]);
    
      async function handleSave() {
        try {
          await updateResource(id, inputName, isActive);
          router.push('/resource/archive');
        } catch (e) {
          setError('Ошибка при сохранении ресурса');
        }
      }
    
      async function handleDelete() {
        try {
          await deleteResource(id);
          router.push('/resource/archive');
        } catch (e) {
          setError('Ошибка при удалении ресурса');
        }
      }
      async function handleActivate() {
        try {
          await updateResource(id, inputName, true);
          router.push('/resource/archive');
        } catch (e) {
          setError('Ошибка при переводе в активные');
        }
      }
    return(
        <main className={styles.mainContainer}>
            <h1 className={styles.headerText}>Ресурс</h1>
            <div className={styles.buttonGroup}>
                <button className={styles.addButton} onClick={handleSave}>Сохранить</button>
                <button className={styles.deleteButton} onClick={handleDelete}>Удалить</button>
                <button className={styles.acceptButton} onClick={handleActivate}>В работу</button>
            </div>
            <div className={styles.inputGroup}>
                <h4 className={styles.filterText}>Наименование</h4>
                <input
                type='text'
                className={styles.inputNumber}
                value={inputName}
                onChange={(e) => setInputName(e.target.value)}
                />
            </div>
        </main>
    )
}