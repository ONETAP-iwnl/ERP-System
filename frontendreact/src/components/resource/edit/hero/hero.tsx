'use client'

import { useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';
import styles from "@/styles/recipts-page/hero/hero.module.css";
import { getResourceById, updateResource, deleteResource, archiveResource } from '@/services/api/resourceClient';

interface Resource {
  id: number;
  name: string;
  isActive: boolean;
}

interface HeroProps {
  id: string;
}

export default function Hero({ id }: HeroProps) {
    const router = useRouter();
    const [resource, setResource] = useState<Resource | null>(null);
    const [inputName, setInputName] = useState('');

    useEffect(() => {
    const resourceId = parseInt(id);
    console.log("Получен ID:", resourceId);

    if (isNaN(resourceId)) {
      console.error("Некорректный ID:", id);
      return;
    }

    async function fetchResource() {
      try {
        const data = await getResourceById(resourceId);
        console.log("Получен ресурс:", data);
        setResource(data);
        setInputName(data.name);
      } catch (error) {
        console.error("Ошибка при получении ресурса", error);
      }
    }

    fetchResource();
  }, [id]);

  const handleSave = async () => {
      if (!resource) return;
      try {
        await updateResource(resource.id, inputName, true);
        router.push('/resources-page');
      } catch (error) {
        console.error('Ошибка при сохранении:', error);
      }
    };
  
    const handleDelete = async () => {
      if (!resource) return;
      try {
        await deleteResource(resource.id);
        router.push('/resources-page');
      } catch (error) {
        console.error('Ошибка при удалении:', error);
      }
    };
  
    const handleArchive = async () => {
      if (!resource) return;
      try {
        await archiveResource(resource.id);
        router.push('/resources-page');
      } catch (error) {
        console.error('Ошибка при архивировании:', error);
      }
    };
  
    if(!resource) return <div>Загрузка...</div>;
    return(
        <main className={styles.mainContainer}>
            <h1 className={styles.headerText}>Ресурс</h1>
            <div className={styles.buttonGroup}>
                <button className={styles.addButton} onClick={handleSave}>Сохранить</button>
                <button className={styles.deleteButton} onClick={handleDelete}>Удалить</button>
                <button className={styles.archiveButton} onClick={handleArchive}>В архив</button>
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